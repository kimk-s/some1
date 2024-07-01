using MemoryPack;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class ObjectGiveStuffFront : IObjectGiveStuffFront, ISyncDestination
    {
        private readonly UnmanagedParticleDestination<bool> _isTaken = new();

        public ReadOnlyReactiveProperty<bool> IsTaken => _isTaken.Value;

        public ReadOnlyReactiveProperty<bool> IsDefault => _isTaken.IsDefault;

        public void Dispose()
        {
            _isTaken.Dispose();
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            _isTaken.Read(ref reader, mode);
        }

        public void Reset()
        {
            _isTaken.Reset();
        }
    }
}
