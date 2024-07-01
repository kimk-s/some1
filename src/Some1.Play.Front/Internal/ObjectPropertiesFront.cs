using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class ObjectPropertiesFront : IObjectPropertiesFront, ISyncDestination
    {
        private readonly SyncArrayDestination _sync;
        private readonly UnmanagedParticleDestination<byte> _team = new();
        private readonly NullableUnmanagedParticleDestination<ObjectPlayer> _player = new();

        internal ObjectPropertiesFront()
        {
            _sync = new SyncArrayDestination(
                _team,
                _player);
        }

        public ReadOnlyReactiveProperty<byte> Team => _team.Value;

        public ReadOnlyReactiveProperty<ObjectPlayer?> Player => _player.Value;

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
