using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class ObjectEnergyFront : IObjectEnergyFront, ISyncDestination
    {
        private readonly SyncArrayDestination _sync;
        private readonly UnmanagedParticleDestination<int> _maxValue = new();
        private readonly UnmanagedParticleDestination<int> _value = new();

        public ObjectEnergyFront(EnergyId id)
        {
            Id = id;
            NormalizedValue = MaxValue.CombineLatest(Value, (max, value) => max == 0 ? 0 : (float)value / max).ToReadOnlyReactiveProperty();

            _sync = new SyncArrayDestination(
                _maxValue,
                _value);
        }

        public EnergyId Id { get; }

        public ReadOnlyReactiveProperty<int> MaxValue => _maxValue.Value;

        public ReadOnlyReactiveProperty<int> Value => _value.Value;

        public ReadOnlyReactiveProperty<float> NormalizedValue { get; }

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        public void Dispose()
        {
            _sync.Dispose();
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
