using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerObjectTraitFront : IPlayerObjectTraitFront, ISyncDestination
    {
        private readonly SyncArrayDestination _sync;
        private readonly UnmanagedParticleDestination<Trait> _next = new();
        private readonly UnmanagedParticleDestination<Trait> _afterNext = new();

        internal PlayerObjectTraitFront()
        {
            _sync = new SyncArrayDestination(
                _next,
                _afterNext);
        }

        public ReadOnlyReactiveProperty<Trait> Next => _next.Value;

        public ReadOnlyReactiveProperty<Trait> AfterNext => _afterNext.Value;

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
