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
    internal sealed class ObjectShift : IObjectShift, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly ReactiveProperty<Shift?> _shift = new();
        private readonly ObjectCycles _cycles;
        private readonly ObjectCast _cast;
        private readonly ObjectWalk _walk;
        private readonly ObjectTransfer _transfer;
        private readonly ObjectTransform _transform;

        internal ObjectShift(
            ObjectCast cast,
            ObjectWalk walk,
            ObjectTransfer transfer,
            ObjectTransform transform,
            ITime time)
        {
            _cast = cast;
            _walk = walk;
            _transfer = transfer;
            _transform = transform;
            _cycles = new(time, (x, _) => x < 1);
            _cycles.EventFired += Cycles_EventFired;
            _sync = new SyncArraySource(
                _shift.ToNullableUnmanagedParticleSource(),
                _cycles);
        }

        public Shift? Shift
        {
            get => _shift.Value;

            private set
            {
                if (Shift == value)
                {
                    return;
                }

                _shift.Value = value;

                _transform.SetShiftRotation(value?.GetTransformRotation());
            }
        }

        ReadOnlyReactiveProperty<Shift?> IObjectShift.Shift => _shift;

        public IObjectCycles Cycles => _cycles;

        internal bool CanSet => Shift is null;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal bool Set(ShiftId id, float rotation, float distance, float time, float speed)
        {
            if (!SpeedHelper.IsValid(distance, time, speed))
            {
                throw new ArgumentOutOfRangeException(nameof(speed));
            }

            if (!CanSet)
            {
                return false;
            }

            Shift = new(id, rotation, distance, time, speed);
            _cycles.Cycle = MathF.Max(0.001f, time);

            if (id.IsStopCast())
            {
                _cast.Stop();
            }

            _walk.Stop();

            return true;
        }

        internal void Update(float deltaSeconds, ParallelToken parallelToken)
        {
            if (Shift is null)
            {
                return;
            }

            float remainDeltaSeconds = _cycles.Update(deltaSeconds, parallelToken);
            if (remainDeltaSeconds > 0)
            {
                Reset();
            }
        }

        internal void Stop()
        {
            if (Shift is null)
            {
                return;
            }

            Shift = null;
            _cycles.Reset();
        }

        internal void Reset()
        {
            Stop();
        }

        private void Cycles_EventFired(object? _, (CycleSpan e, ParallelToken parallelToken) e)
        {
            if (Shift is null)
            {
                throw new InvalidOperationException();
            }

            _transfer.MoveDelta += MoveHelper.CalculateDistance(Shift.Value.Rotation, Shift.Value.Speed, e.e.Length * Shift.Value.Time);
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
