using System;
using System.Buffers;
using MemoryPack;
using Some1.Play.Core.Paralleling;
using Some1.Sync;
using R3;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectCycles : IObjectCycles, ISyncSource
    {
        internal delegate bool ConfirmCyclesUp(int lastCycles, ParallelToken parallelToken);

        private readonly FloatWaveSource _sync;
        private readonly ReactiveProperty<FloatWave> _value = new();
        private readonly ConfirmCyclesUp _confirmCycleUp;
        private float _cycle;
        private float _cycleInverse;
        private int _lastCycles;

        internal ObjectCycles(ITime time, ConfirmCyclesUp confirmCyclesUp)
        {
            _confirmCycleUp = confirmCyclesUp;
            _sync = _value.ToWaveSource(time);
        }

        public ReadOnlyReactiveProperty<FloatWave> Value => _value;

        public float Cycle
        {
            get => _cycle;

            internal set
            {
                value = Math.Max(0, value);
                if (_cycle == value)
                {
                    return;
                }

                Stop();
                _cycle = value;
                _cycleInverse = value == 0 ? 0 : 1 / value;
            }
        }

        public float Time => _value.Value.B * Cycle;

        public bool CanUpdate => Cycle > 0;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        public event EventHandler<(CycleSpan e, ParallelToken parallelToken)>? EventFired;

        public event EventHandler? ScopedReset;

        internal float Update(float deltaTime, ParallelToken parallelToken)
        {
            if (deltaTime <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaTime));
            }

            if (!CanUpdate)
            {
                return deltaTime;
            }

            float remainDeltaTime = 0;
            var value = Value.CurrentValue.Flow(deltaTime * _cycleInverse);
            float a = value.A;
            float b = value.B;

            for (; _lastCycles < b; _lastCycles++)
            {
                if (!_confirmCycleUp(_lastCycles, parallelToken))
                {
                    remainDeltaTime = (b - _lastCycles) * Cycle;
                    b = _lastCycles;
                    break;
                }
            }

            const float MaxValue = 1_000;
            if (b > MaxValue)
            {
                float length = b - a;
                a %= Cycle;
                b = a + length;
            }

            _value.Value = new(a, b);

            EventFired?.Invoke(this, (new(a, b), parallelToken));

            return remainDeltaTime;
        }

        internal void Stop(bool scopedReset = true)
        {
            _value.Value = default;
            _lastCycles = 0;
            if (scopedReset)
            {
               ScopedReset?.Invoke(this, EventArgs.Empty);
            }
        }

        internal void Reset()
        {
            Stop();
            Cycle = 0;
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
