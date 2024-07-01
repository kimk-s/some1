using System;
using System.Buffers;
using MemoryPack;
using R3;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectBooster : IObjectBooster, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly BoosterInfo _info;
        private readonly ObjectStatGroup _stats;
        private readonly ReactiveProperty<int> _number = new();
        private readonly ReactiveProperty<FloatWave> _cycles = new();
        private int _cycleNumber;

        internal ObjectBooster(BoosterInfo info, ObjectStatGroup stats, ITime time)
        {
            _info = info;
            _stats = stats;
            _sync = new SyncArraySource(
                _number.ToUnmanagedParticleSource(),
                _cycles.ToWaveSource(time));
        }

        public BoosterId Id => _info.Id;

        public int Number
        {
            get => _number.Value;

            private set
            {
                value = Math.Clamp(value, 0, PlayConst.MaxBoosterNumber);
                if (Number == value)
                {
                    return;
                }

                _number.Value = value;

                switch (Id)
                {
                    case BoosterId.Power:
                        _stats.SetBoosterValue(StatId.Offense, value);
                        _stats.SetBoosterValue(StatId.Health, value);
                        break;
                    case BoosterId.Accel:
                        _stats.SetBoosterValue(StatId.Accel, value);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public FloatWave Cycles => _cycles.Value;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal void Add(int number)
        {
            if (number == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(number));
            }

            Number += number;
            StopCycles();
            if (Number != 0)
            {
               _cycleNumber = 1;
            }
        }

        internal void Update(float deltaSeconds)
        {
            if (deltaSeconds <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds));
            }

            if (Number == 0)
            {
                return;
            }

            _cycles.Value = _cycles.Value.Flow(deltaSeconds / _info.Seconds);
            for (; _cycleNumber < Cycles.B; _cycleNumber++)
            {
                if (--Number == 0)
                {
                    Reset();
                    break;
                }
            }
        }

        internal void Reset()
        {
            Stop();
        }

        internal void Stop()
        {
            Number = 0;
            StopCycles();
        }

        private void StopCycles()
        {
            _cycles.Value = default;
            _cycleNumber = 0;
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
