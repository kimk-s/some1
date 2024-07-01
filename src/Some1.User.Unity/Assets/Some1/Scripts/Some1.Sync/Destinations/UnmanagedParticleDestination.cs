using MemoryPack;

namespace Some1.Sync.Destinations
{
    public sealed class UnmanagedParticleDestination<T> : SyncParticleDestination<T> where T : unmanaged
    {
        protected sealed override void ReadValue(ref MemoryPackReader reader) => ProtectedValue.Value = reader.ReadUnmanaged<T>();
    }
}
