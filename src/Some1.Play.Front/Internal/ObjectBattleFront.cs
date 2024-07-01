using MemoryPack;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class ObjectBattleFront : IObjectBattleFront, ISyncDestination
    {
        private readonly NullableUnmanagedParticleDestination<bool> _battle = new();

        public ReadOnlyReactiveProperty<bool?> Battle => _battle.Value;

        public ReadOnlyReactiveProperty<bool> IsDefault => _battle.IsDefault;

        public void Dispose()
        {
            _battle.Dispose();
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            _battle.Read(ref reader, mode);
        }

        public void Reset()
        {
            _battle.Reset();
        }
    }
}
