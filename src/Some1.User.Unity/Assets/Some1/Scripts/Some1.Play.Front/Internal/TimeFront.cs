using System;
using MemoryPack;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;
using Some1.Play.Info;

namespace Some1.Play.Front.Internal
{
    internal sealed class TimeFront : ITimeFront, ISyncReadable, ISyncPolatable, IDisposable
    {
        private readonly SyncArrayDestination _sync;
        private readonly UnmanagedParticleDestination<int> _frameCount = new();
        private readonly UnmanagedParticleDestination<FloatWave> _deltaSeconds = new();
        private readonly DoubleWaveDestination _totalSeconds;
        private readonly DoubleWaveDestination _utcNowSeconds;
        private readonly ReadOnlyReactiveProperty<FloatWave> _fps;

        internal TimeFront()
        {
            _totalSeconds = new(this);
            _utcNowSeconds = new(this);

            UtcNowSeconds = _utcNowSeconds.InterpolatedValue;
            UtcNow = UtcNowSeconds.Select(x => PlayConst.StandardDateTime.AddSeconds(x)).ToReadOnlyReactiveProperty();

            _fps = _deltaSeconds.Value.Select(x => x.Inverse()).ToReadOnlyReactiveProperty();

            _sync = new(
                _frameCount,
                _deltaSeconds,
                _totalSeconds,
                _utcNowSeconds);
        }

        public ReadOnlyReactiveProperty<int> FrameCount => _frameCount.Value;

        public ReadOnlyReactiveProperty<DoubleWave> TotalSecondsWave => _totalSeconds.Value;

        public ReadOnlyReactiveProperty<double> TotalSeconds => _totalSeconds.InterpolatedValue;

        public ReadOnlyReactiveProperty<double> UtcNowSeconds { get; }

        public ReadOnlyReactiveProperty<DateTime> UtcNow { get; }

        int ISyncTime.FrameCount => FrameCount.CurrentValue;

        FloatWave ISyncTime.DeltaTime => _deltaSeconds.Value.CurrentValue;

        FloatWave ISyncTime.FPS => _fps.CurrentValue;

        public void Dispose()
        {
            _sync.Dispose();
        }

        public void Extrapolate()
        {
            _sync.Extrapolate();
        }

        public void Interpolate(float time)
        {
            _sync.Interpolate(time);
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            _sync.Read(ref reader, mode);
        }
    }
}
