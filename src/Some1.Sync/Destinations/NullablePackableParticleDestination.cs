using MemoryPack;

namespace Some1.Sync.Destinations
{
    public sealed class NullablePackableParticleDestination<T> : SyncParticleDestination<T?> where T : struct, IMemoryPackable<T>
    {
        protected override void ReadValue(ref MemoryPackReader reader) => ProtectedValue.Value = reader.ReadPackable<T>();
    }
}
