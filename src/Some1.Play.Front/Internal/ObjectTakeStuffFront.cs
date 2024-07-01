using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class ObjectTakeStuffFront : IObjectTakeStuffFront, ISyncDestination, ISyncPolatable
    {
        private readonly SyncArrayDestination _sync;
        private readonly NullableUnmanagedParticleDestination<Stuff> _stuff = new();
        private readonly FloatWaveDestination _cycles;

        internal ObjectTakeStuffFront(ISyncTime syncFrame)
        {
            _sync = new(
                _stuff,
                _cycles = new(syncFrame));
        }

        public ReadOnlyReactiveProperty<Stuff?> Stuff => _stuff.Value;

        public ReadOnlyReactiveProperty<float> Cycles => _cycles.InterpolatedValue;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

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
