using MemoryPack;

namespace Some1.Sync.Destinations
{
    public sealed class ValueParticleDestination<T> : SyncParticleDestination<T?>
    {
        protected override void ReadValue(ref MemoryPackReader reader) => ProtectedValue.Value = reader.ReadValue<T>();
    }
}
