using System;
using System.Buffers;
using MemoryPack;
using Some1.Sync;
using Some1.Sync.Sources;
using R3;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class Time : ITime, ISyncWritable
    {
        private readonly SyncArraySource _sync;
        private readonly IClock _clock;
        private readonly ReactiveProperty<int> _frameCount = new();
        private readonly ReactiveProperty<FloatWave> _deltaSeconds = new();
        private readonly ReactiveProperty<DoubleWave> _totalSeconds = new();
        private readonly ReactiveProperty<DoubleWave> _utcNowSeconds = new();
        private FloatWave _fps;

        internal Time(IClock clock)
        {
            _clock = clock;
            _sync = new SyncArraySource(
                _frameCount.ToUnmanagedParticleSource(),
                _deltaSeconds.ToUnmanagedParticleSource(),
                _totalSeconds.ToWaveSource(this),
                _utcNowSeconds.ToWaveSource(this));
        }

        public int FrameCount => _frameCount.Value;

        public float DeltaSeconds => _deltaSeconds.Value.B;

        public double TotalSeconds => _totalSeconds.Value.B;

        public double UtcNowSeconds => _utcNowSeconds.Value.B;

        public DateTime UtcNow { get; private set; }

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        FloatWave ISyncTime.DeltaTime => _deltaSeconds.Value;

        FloatWave ISyncTime.FPS => _fps;

        public void Write<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, SyncMode mode) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
        {
            _sync.Write(ref writer, mode);
        }

        internal void Update(float deltaSeconds)
        {
            if (deltaSeconds <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds));
            }

            _sync.ClearDirty();

            _frameCount.Value++;

            _deltaSeconds.Value = new(_deltaSeconds.Value.B, deltaSeconds);
            _fps = _deltaSeconds.Value.Inverse();

            _totalSeconds.Value = _totalSeconds.Value.Flow(deltaSeconds);

            var utcNowSeconds = _utcNowSeconds.Value.Flow(deltaSeconds);
            var clockSeconds = (_clock.UtcNow - PlayConst.StandardDateTime).TotalSeconds;
            _utcNowSeconds.Value = Math.Abs(utcNowSeconds.B - clockSeconds) > 1 ? new(clockSeconds) : utcNowSeconds;

            UtcNow = PlayConst.StandardDateTime.AddSeconds(_utcNowSeconds.Value.B);
        }
    }
}
