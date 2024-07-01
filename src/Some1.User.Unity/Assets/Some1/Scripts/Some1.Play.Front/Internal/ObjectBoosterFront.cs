using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class ObjectBoosterFront : IObjectBoosterFront, ISyncDestination, ISyncPolatable
    {
        private readonly BoosterInfo _info;
        private readonly SyncArrayDestination _sync;
        private readonly UnmanagedParticleDestination<int> _number = new();
        private readonly FloatWaveDestination _cycles;

        internal ObjectBoosterFront(BoosterInfo info, ISyncTime syncFrame)
        {
            _info = info ?? throw new System.ArgumentNullException(nameof(info));
            _cycles = new(syncFrame);

            NormalizedConsumingDelay = Cycles.Select(x => x == 0 ? 0 : 1 - (x % 1)).ToReadOnlyReactiveProperty();

            Time = Cycles.Select(x => x * _info.Seconds).ToReadOnlyReactiveProperty();

            _sync = new SyncArrayDestination(
                _number,
                _cycles);
        }

        public BoosterId Id => _info.Id;

        public ReadOnlyReactiveProperty<int> Number => _number.Value;

        public ReadOnlyReactiveProperty<bool> IsDefault => _number.IsDefault;

        public ReadOnlyReactiveProperty<float> Cycles => _cycles.InterpolatedValue;

        public ReadOnlyReactiveProperty<float> NormalizedConsumingDelay { get; }

        public ReadOnlyReactiveProperty<float> Time { get; }

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

        public void Reset()
        {
            _sync.Reset();
        }
    }
}
