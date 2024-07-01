using MemoryPack;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerWelcomeFront : IPlayerWelcomeFront, ISyncDestination
    {
        private readonly UnmanagedParticleDestination<bool> _welcome = new();

        public ReadOnlyReactiveProperty<bool> Welcome => _welcome.Value;

        public ReadOnlyReactiveProperty<bool> IsDefault => _welcome.IsDefault;

        public void Dispose()
        {
            _welcome.Dispose();
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            _welcome.Read(ref reader, mode);
        }

        public void Reset()
        {
            _welcome.Reset();
        }
    }
}
