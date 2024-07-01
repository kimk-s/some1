using System;
using System.Buffers;
using MemoryPack;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;
using R3;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectWalk : IObjectWalk, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly ObjectStatGroup _stats;
        private readonly ObjectTransfer _transfer;
        private readonly ObjectTransform _transform;
        private readonly ObjectProperties _properties;
        private readonly Space _space;
        private readonly ReactiveProperty<Walk?> _walk = new();
        private readonly ObjectCycles _cycles;
        private CharacterWalkInfo? _info;
        private CharacterMoveInfo? _moveInfo;

        internal ObjectWalk(
            ObjectStatGroup stats,
            ObjectTransfer transfer,
            ObjectTransform trnasform,
            ObjectProperties properties,
            Space space,
            ITime time)
        {
            _stats = stats;
            _transfer = transfer;
            _transform = trnasform;
            _properties = properties;
            _space = space;
            _cycles = new(time, (_, __) => true);
            _cycles.EventFired += Cycles_EventFired;
            _sync = new SyncArraySource(
                _walk.ToNullableUnmanagedParticleSource(),
                _cycles);

            stats.All[StatId.StunWalk].ValueChanged += (_, e) =>
            {
                if (e > 0)
                {
                    Stop();
                }
            };
        }

        public Walk? Walk
        {
            get => _walk.Value;
            
            private set
            {
                if (Walk == value)
                {
                    return;
                }

                _walk.Value = value;

                _transform.SetWalkRotation(value?.Rotation);
            }
        }

        public float Cycles => _cycles.Value.CurrentValue.B;

        internal bool IsRunning => Walk?.IsOn ?? false;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal void Set(CharacterWalkInfo? info, CharacterMoveInfo? moveInfo)
        {
            _info = info;
            _moveInfo = moveInfo;
            _cycles.Cycle = _info?.Cycle ?? 0;
        }

        internal void Set(Walk? walk)
        {
            if (_info is null)
            {
                return;
            }

            if (Walk == walk)
            {
                return;
            }

            if (walk is not null && walk.Value.IsOn)
            {
                Walk = walk;
            }
            else
            {
                Stop();
            }
        }

        internal bool TrySetScan(float length, ObjectTarget objectTarget, ParallelToken parallelToken)
        {
            var rotation = GetScanRotation(length, objectTarget, parallelToken);
            if (rotation is null)
            {
                return false;
            }

            Set(new Walk(true, rotation.Value));
            
            return true;
        }

        private float? GetScanRotation(float length, ObjectTarget objectTarget, ParallelToken parallelToken) => _moveInfo is null
            ? null
            : _space.GetWalkScanAim(
                new(
                    _properties.Area,
                    length,
                    objectTarget,
                    _moveInfo.BumpLevel),
                parallelToken)?.Rotation;

        internal void Reset()
        {
            Stop();
            _info = null;
            _moveInfo = null;
            _cycles.Reset();
        }

        internal void Stop()
        {
            Walk = null;
            _cycles.Stop();
        }

        internal void Update(float deltaSeconds, ParallelToken parallelToken)
        {
            if (_info is null || Walk is null || !Walk.Value.IsOn)
            {
                return;
            }

            deltaSeconds = StatHelper.Calculate(deltaSeconds, GetAccel());
            _cycles.Update(deltaSeconds, parallelToken);
        }

        private void Cycles_EventFired(object? _, (CycleSpan e, ParallelToken? parallelToken) e)
        {
            if (_info is null || Walk is null || !Walk.Value.IsOn)
            {
                throw new InvalidOperationException();
            }

            _transfer.MoveDelta += MoveHelper.CalculateDistance(Walk.Value.Rotation, _info.Speed, e.e.Length * _info.Cycle);
            _transform.SetWalkRotation(Walk.Value.Rotation);
        }

        private int GetAccel()
        {
            return _stats.All[StatId.Accel].Value;
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
