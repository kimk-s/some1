using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerObjectSpecialtyFront : IPlayerObjectSpecialtyFront, ISyncDestination
    {
        private readonly NullableUnmanagedParticleDestination<SpecialtyPacket> _specialty = new();

        public ReadOnlyReactiveProperty<SpecialtyPacket?> Specialty => _specialty.Value;

        public ReadOnlyReactiveProperty<bool> IsDefault => _specialty.IsDefault;

        public void Dispose()
        {
            _specialty.Dispose();
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            _specialty.Read(ref reader, mode);
        }

        public void Reset()
        {
            _specialty.Reset();
        }
    }
}
