using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class ObjectWalkFront : IObjectWalkFront, ISyncDestination, ISyncPolatable
    {
        private readonly SyncArrayDestination _sync;
        private readonly NullableUnmanagedParticleDestination<Walk> _walk = new();
        private readonly FloatWaveDestination _cycles;

        internal ObjectWalkFront(ISyncTime syncFrame, ReadOnlyReactiveProperty<CharacterWalkInfo?> info)
        {
            _sync = new(
                _walk,
                _cycles = new(syncFrame));

            Cycle = info.Select(x => x?.Cycle ?? 0).ToReadOnlyReactiveProperty();
        }

        public ReadOnlyReactiveProperty<Walk?> Walk => _walk.Value;

        public ReadOnlyReactiveProperty<float> Cycles => _cycles.InterpolatedValue;

        public ReadOnlyReactiveProperty<float> Cycle { get; }

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
