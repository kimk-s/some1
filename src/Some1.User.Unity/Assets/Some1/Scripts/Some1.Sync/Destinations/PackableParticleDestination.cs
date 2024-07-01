using MemoryPack;

namespace Some1.Sync.Destinations
{
    public sealed class PackableParticleDestination<T> : SyncParticleDestination<T?> where T : IMemoryPackable<T>
    {
        protected override void ReadValue(ref MemoryPackReader reader) => ProtectedValue.Value = reader.ReadPackable<T>();
    }
}
