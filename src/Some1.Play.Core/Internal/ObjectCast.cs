using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MemoryPack;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;
using R3;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectCast : IObjectCast, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly ReactiveProperty<Cast?> _current = new();
        private readonly ObjectStatGroup _stats;
        private readonly ObjectTransform _transform;
        private readonly ObjectTrait _trait;
        private readonly ObjectCastItem[] _items;
        private readonly ObjectCycles _cycles;
        private short _autoToken;
        private ObjectCastItem? _item;

        internal ObjectCast(
            int objectId,
            CharacterCastInfoGroup infos,
            CharacterCastStatInfoGroup statInfos,
            ObjectStatGroup stats,
            ObjectTransform transform,
            ObjectTrait trait,
            IObjectProperties properties,
            Space space,
            ITime time)
        {
            _items = EnumForUnity.GetValues<CastId>()
                .Select(x => new ObjectCastItem(
                    objectId,
                    x,
                    infos,
                    statInfos,
                    transform,
                    properties,
                    space))
                .ToArray();
            Items = _items.ToDictionary(x => x.Id, x => (IObjectCastItem)x);
            _cycles = new(time, Cycles_ConfirmCyclesUp);
            _stats = stats;
            _transform = transform;
            _trait = trait;
            _sync = new SyncArraySource(
                _current.Select(x => x is null ? null : (CastPacket?)new CastPacket(x.Value.Id, x.Value.Aim))
                    .ToReadOnlyReactiveProperty().ToNullableUnmanagedParticleSource(),
                _cycles);

            _stats.All[StatId.StunCast].ValueChanged += (_, e) =>
            {
                if (e > 0)
                {
                    Stop();
                    Next = null;
                }
            };
        }

        public IReadOnlyDictionary<CastId, IObjectCastItem> Items { get; }

        public Cast? Next { get; private set; }

        public Cast? Current
        {
            get => _current.Value;

            private set
            {
                if (Current == value)
                {
                    return;
                }
                if (Info is null)
                {
                    throw new InvalidOperationException();
                }

                if (value is null)
                {
                    _current.Value = value;
                }
                else
                {
                    _current.Value = new(
                        value.Value.Id,
                        value.Value.Token,
                        value.Value.IsOn,
                        new(
                            Info.AimLimit.UseRotation ? value.Value.Aim.Rotation : _transform.Rotation.CurrentValue,
                            Math.Clamp(value.Value.Aim.Length, Info.AimLimit.MinLength, Info.AimLimit.MaxLength)));
                }

                _transform.SetCastRotation(_current.Value?.Aim.Rotation);
            }
        }

        ReadOnlyReactiveProperty<Cast?> IObjectCast.Current => _current;

        public IObjectCycles Cycles => _cycles;

        internal int Token { get; private set; }

        private ObjectCastItem? Item
        {
            get => _item;

            set
            {
                _item = value;

                _cycles.Cycle = Info?.Cycle ?? 0;
                _cycles.Stop();

                _trait.SetCastValue(((Info?.Trait?.Condition ?? 0) & _trait.Next.CurrentValue) != 0);

                if (StatInfos is null)
                {
                    _stats.ResetCastValue();
                }
                else
                {
                    for (int i = 0; i < StatInfos.Count; i++)
                    {
                        var item = StatInfos[i];
                        if ((item.Condition & _trait.Value) != 0)
                        {
                            _stats.SetCastValue(item.StatId, item.StatValue);
                        }
                    }
                }
            }
        }

        private CharacterCastInfo? Info => Item?.Info;

        private IReadOnlyList<CharacterCastStatInfo>? StatInfos => Item?.StatInfos;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal void Set(CharacterId characterId)
        {
            TryStop();
            foreach (var item in _items)
            {
                item.Set(characterId);
            }
        }

        internal void Set(Cast? next)
        {
            Next = next;
        }

        private ObjectCastItem? GetItem(CastId id)
        {
            int index = (int)id;
            if (index < 0 || index >= _items.Length)
            {
                return null;
            }

            return _items[index];
        }

        internal bool TrySetScan(CastId id, ParallelToken parallelToken)
        {
            var aim = GetItem(id)?.GetScanAim(parallelToken);
            if (aim is null)
            {
                return false;
            }

            Set(new Cast(
                id,
                IssueAutoToken(ref _autoToken),
                false,
                aim.Value));

            return true;

            static short IssueAutoToken(ref short token)
            {
                if (token < PlayConst.MinCastAutoToken || token >= PlayConst.MaxCastAutoToken)
                {
                    token = PlayConst.MinCastAutoToken;
                }
                else
                {
                    token++;
                }
                return token;
            }
        }

        internal bool CanConsume(CastId id) => GetItem(id)?.CanConsume() ?? false;

        internal void Update(float deltaSeconds, ParallelToken parallelToken)
        {
            if (deltaSeconds <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds));
            }

            foreach (var item in _items)
            {
                item.Update(deltaSeconds);
            }

            _ = TryCancel();

            while (deltaSeconds > 0)
            {
                if (Info is null)
                {
                    if (!TryStart(parallelToken))
                    {
                        break;
                    }
                }
                else
                {
                    deltaSeconds = Advance(deltaSeconds, parallelToken);
                }
            }
        }

        internal void Stop()
        {
            TryStop();
        }

        internal void Reset()
        {
            Current = null;
            Item = null;
            foreach (var item in _items)
            {
                item.Reset();
            }
            Next = null;
            _cycles.Reset();
            _autoToken = 0;
        }

        internal void SetBattle(bool value)
        {
            Stop();

            foreach (var item in _items)
            {
                item.Battle = value;
            }
        }

        internal void SetIdle(bool value)
        {
            foreach (var item in _items)
            {
                item.Idle = value;
            }
        }

        internal void FillCharge()
        {
            foreach (var item in _items)
            {
                item.FillLoad();
            }
        }

        internal void ClearCharge()
        {
            foreach (var item in _items)
            {
                item.ClearLoad();
            }
        }

        private bool TryStart(ParallelToken parallelToken)
        {
            if (Next is null
                || !Next.Value.IsOn && Next.Value.Token == Token
                || Item is not null)
            {
                return false;
            }

            Token = Next.Value.Token;

            var item = GetItem(Next.Value.Id);
            if (item?.Info is null)
            {
                return false;
            }

            if (!item.Info.UseOn && Next.Value.IsOn)
            {
                return false;
            }

            if (!item.TryConsume())
            {
                return false;
            }

            Item = item;
            SetCurrentToNext(parallelToken);

            return true;
        }

        private bool TryStop()
        {
            foreach (var item in _items)
            {
                item.Stop();
            }

            if (Info is null)
            {
                return false;
            }

            Current = null;
            Item = null;
            return true;
        }

        private bool TryCancel()
        {
            if (Info is null || !Info.UseOn || Current is null || Current.Value.IsOn)
            {
                return false;
            }
            var result = TryStop();
            Debug.Assert(result);
            return true;
        }

        private bool TryEdit(ParallelToken parallelToken)
        {
            if (Info is null
                || Next is null
                || Next.Value.Id != Info.Id.Cast
                || Next.Value.Token == Token)
            {
                return false;
            }
            SetCurrentToNext(parallelToken);
            return true;
        }

        private float Advance(float deltaSeconds, ParallelToken parallelToken)
        {
            if (deltaSeconds <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds));
            }

            if (Info is null || Item is null)
            {
                throw new InvalidOperationException();
            }

            if (Info.UseOn)
            {
                TryEdit(parallelToken);
            }

            float remainDeltaSeconds = _cycles.Update(deltaSeconds, parallelToken);
            if (remainDeltaSeconds > 0)
            {
                TryStop();
            }

            return remainDeltaSeconds;
        }

        private bool Cycles_ConfirmCyclesUp(int lastCycles, ParallelToken parallelToken)
        {
            if (Info is null || Item is null)
            {
                throw new InvalidOperationException();
            }

            if (lastCycles > 0)
            {
                if (!Info.UseOn && !TryEdit(parallelToken))
                {
                    return false;
                }

                if (!Item.TryConsume())
                {
                    return false;
                }

                SetCurrentToNext(parallelToken);
            }

            return true;
        }

        private void SetCurrentToNext(ParallelToken parallelToken)
        {
            if (Item is null)
            {
                throw new InvalidOperationException();
            }

            if (Next is null)
            {
                return;
            }

            if (Current == Next)
            {
                return;
            }

            var aim = Next.Value.Aim.IsAuto
                ? Item.GetAutoAim(parallelToken)
                : Next.Value.Aim;

            Current = new(
                Next.Value.Id,
                Next.Value.Token,
                Next.Value.IsOn,
                aim);

            Token = Current.Value.Token;
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
