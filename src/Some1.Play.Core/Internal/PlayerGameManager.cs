using System;
using System.Buffers;
using System.Linq;
using MemoryPack;
using R3;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class PlayerGameManager : IPlayerGameManager, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly ReactiveProperty<GameManagerStatus?> _status = new();
        private readonly ReactiveProperty<FloatWave> _cycles = new();
        private readonly PlayerGameArgsGroup _argses;
        private readonly PlayerGame _game;
        private readonly PlayerGameResultGroup _results;
        private readonly PlayerExp _exp;
        private readonly PlayerCharacterGroup _characters;
        private readonly PlayerSpecialtyGroup _specialties;
        private readonly Object _object;
        private readonly IRegionGroup _regions;
        private readonly ITime _time;

        private int _previousObjectHealth;

        private bool _isPlayingToSave;

        internal PlayerGameManager(
            PlayerGameArgsGroup argses,
            PlayerGame game,
            PlayerGameResultGroup results,
            PlayerExp exp,
            PlayerCharacterGroup characters,
            PlayerSpecialtyGroup specialties,
            Object @object,
            IRegionGroup regions,
            ITime time)
        {
            _argses = argses;
            _game = game;
            _results = results;
            _exp = exp;
            _characters = characters;
            _specialties = specialties;
            _object = @object;
            _regions = regions;
            _time = time;
            _sync = new SyncArraySource(
                _status.ToNullableUnmanagedParticleSource(),
                _cycles.ToWaveSource(time));

            _object.Shift.Shift.Subscribe(Object_ShiftChanged);
            _object.Cast.Current.Subscribe(Object_CastChanged);
            _object.Energies[EnergyId.Health].Value.Subscribe(Object_HealthEnergyValueChanged);
            _object.Transform.Position.Subscribe(Object_TransformPositionChanged);
        }

        public GameManagerStatus? Status
        {
            get => _status.Value;

            private set
            {
                _status.Value = value;

                _cycles.Value = default;

                switch (value)
                {
                    case GameManagerStatus.Ready:
                    case GameManagerStatus.Return:
                        _object.AddBuffBySelf(BuffId.Buff12);
                        break;
                    case GameManagerStatus.ReReady:
                    case GameManagerStatus.ReturnFaulted:
                    case null:
                    default:
                        _object.StopBuff(BuffId.Buff12);
                        break;
                }
            }
        }

        public float Cycles => _cycles.Value.B;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal bool IsPlayingToSave
        {
            get => _isPlayingToSave;

            set
            {
                if (_isPlayingToSave == value)
                {
                    return;
                }

                _isPlayingToSave = value;
                
                TryStop(false);
            }
        }

        internal void SelectArgs(GameMode id)
        {
            if (Status.IsRuning() || _game.Game is not null)
            {
                return;
            }

            _argses.Select(id);
        }

        internal void Ready()
        {
            if (Status.IsRuning() || _game.Game is not null)
            {
                return;
            }

            Status = GameManagerStatus.Ready;
        }

        internal void Return()
        {
            if (Status.IsRuning() || _game.Game is null || _game.Game.Value.Mode == GameMode.Challenge || !_object.Alive.Alive.CurrentValue)
            {
                return;
            }

            if (_object.Shift.Shift.CurrentValue is not null || _object.Cast.Current.CurrentValue is not null || _object.Walk.Walk?.IsOn == true)
            {
                Status = GameManagerStatus.ReturnFaulted;
                return;
            }

            Status = GameManagerStatus.Return;
        }

        internal void Cancel()
        {
            if (!Status.IsCancellable())
            {
                return;
            }

            if (Status == GameManagerStatus.Return)
            {
                if (_game.Game is null || _game.Game.Value.Mode == GameMode.Challenge || !_object.Alive.Alive.CurrentValue)
                {
                    return;
                }
            }

            Status = null;
        }

        internal void Update(float deltaSeconds, ParallelToken parallelToken)
        {
            if (IsPlayingToSave)
            {
                UpdateByRegionSection();
                UpdateByDead();
                FlowCycle(deltaSeconds, parallelToken);
                UpdateByChallengeTime();
            }
            else
            {
                if (_object.Region.Section?.Type.IsBattle() == true)
                {
                    WarpToTown(parallelToken);
                }
            }
        }

        private void UpdateByRegionSection()
        {
            if (_object.Region.Section?.Type.IsNonBattle() == true)
            {
                TryStop();
            }
            else
            {
                TryStart(GameMode.Adventure);
            }
        }

        private void UpdateByDead()
        {
            if (Status.IsRuning() || _game.Game is null || _object.Alive.Alive.CurrentValue)
            {
                return;
            }

            Status = _game.Game.Value.Mode switch
            {
                GameMode.Challenge => GameManagerStatus.ReReady,
                GameMode.Adventure => GameManagerStatus.Return,
                _ => throw new InvalidOperationException(),
            };
        }

        private void UpdateByChallengeTime()
        {
            if (Status == GameManagerStatus.Return
                || _game.Game is null
                || _game.Game.Value.Mode != GameMode.Challenge
                || _game.Cycles < PlayConst.ChallengeReturnTime)
            {
                return;
            }

            Status = GameManagerStatus.Return;
        }

        private void FlowCycle(float deltaSeconds, ParallelToken parallelToken)
        {
            if (Status is null)
            {
                return;
            }

            _cycles.Value = _cycles.Value.Flow(deltaSeconds);

            switch (Status)
            {
                case GameManagerStatus.Ready:
                    {
                        if (Cycles > PlayConst.PlayerGameManagerReadySeconds)
                        {
                            if (!TryStart(_argses.Selected.Id))
                            {
                                throw new InvalidOperationException();
                            }

                            WarpToField(parallelToken);
                        }
                    }
                    break;
                case GameManagerStatus.Return:
                    {
                        if (Cycles > PlayConst.PlayerGameManagerReturnSeconds)
                        {
                            WarpToTown(parallelToken);

                            if (!TryStop())
                            {
                                throw new InvalidOperationException();
                            }
                        }
                    }
                    break;
                case GameManagerStatus.ReReady:
                    {
                        if (Cycles > PlayConst.PlayerGameManagerReReadySeconds)
                        {
                            _object.SetEnergiesValueRate(0.5f);
                            Status = null;
                            WarpToField(parallelToken);
                        }
                    }
                    break;
                case GameManagerStatus.ReturnFaulted:
                    {
                        if (Cycles > PlayConst.PlayerGameManagerReturnFaultedSeconds)
                        {
                            Status = null;
                        }
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        internal void Reset()
        {
            Status = null;
            _isPlayingToSave = false;
        }

        private bool TryStart(GameMode mode)
        {
            if (_game.Game is not null)
            {
                return false;
            }

            _game.Set(mode);
            _object.SetBattle(true);
            Status = null;
            return true;
        }

        private bool TryStop(bool? success = null)
        {
            if (_game.Game is null)
            {
                return false;
            }

            bool valid = _game.Game.Value.Score > 0;
            if (valid)
            {
                success ??= _object.Alive.Alive.CurrentValue;
                int exp = success.Value ? _exp.Consume(_game.Game.Value.Score) : 0;

                var character = new GameResultCharacter(
                    _characters.Selected.Id,
                    exp);
                var specialties = _object.Specialties
                    .Where(x => x.Id.CurrentValue is not null)
                    .OrderByDescending(x => x.Token.CurrentValue)
                    .Select(x => new GameResultSpecialty(x.Id.CurrentValue!.Value, x.Number.CurrentValue))
                    .ToArray();

                _results.Add(new(
                    success.Value,
                    _game.Game.Value,
                    _game.Game.Value.Mode == GameMode.Challenge ? PlayConst.ChallengeSeconds : _game.Cycles,
                    _time.UtcNow,
                    character,
                    specialties));

                if (success.Value)
                {
                    _argses.TrySetScore(_game.Game.Value.Mode, _game.Game.Value.Score);

                    _characters.AddExp(exp);

                    foreach (var item in specialties)
                    {
                        _specialties.Add(item.Id, item.Number);
                    }
                }

                _characters.ReElectIfRandomSkin();
            }

            _game.Reset();
            _object.SetBattle(false);
            Status = null;
            return true;
        }

        private void Object_ShiftChanged(Shift? value)
        {
            if (value is not null)
            {
                TryFaultReturn();
            }
        }

        private void Object_CastChanged(Cast? value)
        {
            if (value is not null)
            {
                TryFaultReturn();
            }
        }

        private void Object_HealthEnergyValueChanged(int value)
        {
            if (_previousObjectHealth > value)
            {
                TryFaultReturn();
            }

            _previousObjectHealth = value;
        }

        private void Object_TransformPositionChanged(Vector2Wave value)
        {
            TryFaultReturn();
        }

        private void TryFaultReturn()
        {
            if (Status != GameManagerStatus.Return || _game.Game is null || _game.Game.Value.Mode == GameMode.Challenge)
            {
                return;
            }

            Status = GameManagerStatus.ReturnFaulted;
        }

        public void ClearDirty()
        {
            _sync.ClearDirty();
        }

        public void Write<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, SyncMode mode) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
        {
            _sync.Write(ref writer, mode);
        }

        public void Dispose()
        {
            _sync.Dispose();
        }

        private void WarpToTown(ParallelToken parallelToken)
        {
            _object.WarpTransfer(_regions.GetTownWarpPosition(_object.Properties.Area.Position), parallelToken);
        }

        private void WarpToField(ParallelToken parallelToken)
        {
            _object.WarpTransfer(_regions.GetFieldWarpPosition(_object.Properties.Area.Position), parallelToken);
        }
    }
}
