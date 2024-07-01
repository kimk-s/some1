using MemoryPack;

namespace Some1.Sync.Destinations
{
    public sealed class StringParticleDestination : SyncParticleDestination<string?>
    {
        protected override void ReadValue(ref MemoryPackReader reader) => ProtectedValue.Value = reader.ReadString();
    }
}
