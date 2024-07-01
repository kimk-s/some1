using System;
using System.Buffers;
using System.Linq;
using MemoryPack;
using R3;
using Some1.Play.Data;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class PlayerGameResult : IPlayerGameResult, ISyncSource
    {
        private readonly NullablePackableParticleSource<GameResult> _sync;
        private readonly ReactiveProperty<GameResult?> _result = new();
        private readonly GameResultData _save;

        public PlayerGameResult()
        {
            _sync = _result.ToNullablePackableParticleSource();
            _save = new()
            {
                Specialties = new()
            };
        }

        public GameResult? Result { get => _result.Value; private set => _result.Value = value; }

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal void Load(IGameResultData data)
        {
            if (data.Game is null)
            {
                return;
            }

            Set(new(
                data.Success,
                new((GameMode)data.Game.Value.Mode, data.Game.Value.Score),
                data.Cycles,
                data.EndTime,
                new((CharacterId)data.Character.Id, data.Character.Exp),
                data.Specialties.Select(x => new GameResultSpecialty((SpecialtyId)x.Id, x.Number)).ToArray()));
        }

        internal IGameResultData Save()
        {
            var save = _save;

            if (Result is null)
            {
                save.Success = false;
                save.Game = null;
                save.Cycles = 0;
                save.EndTime = DateTime.MinValue;
                save.Character = new();
                save.Specialties.Clear();
            }
            else
            {
                save.Success = Result.Value.Success;
                save.Game = new((int)Result.Value.Game.Mode, Result.Value.Game.Score);
                save.Cycles = Result.Value.Cycles;
                save.EndTime = Result.Value.EndTime;
                save.Character = new((int)Result.Value.Character.Id, Result.Value.Character.Exp);

                save.Specialties.Clear();
                foreach (var item in Result.Value.Specialties)
                {
                    save.Specialties.Add(new(
                        (int)item.Id,
                        item.Number));
                }
            }

            return save;
        }

        internal void Set(GameResult result)
        {
            Result = result;
        }

        internal void Reset()
        {
            Result = null;
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
    }
}
