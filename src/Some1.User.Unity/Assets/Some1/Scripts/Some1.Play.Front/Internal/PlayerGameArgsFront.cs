using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerGameArgsFront : IPlayerGameArgsFront, ISyncDestination
    {
        private readonly SyncArrayDestination _sync;
        private readonly UnmanagedParticleDestination<bool> _isSelected = new();
        private readonly UnmanagedParticleDestination<int> _score = new();

        internal PlayerGameArgsFront(GameMode id)
        {
            Id = id;
            _sync = new SyncArrayDestination(
                _isSelected,
                _score);
        }

        public GameMode Id { get; }

        public ReadOnlyReactiveProperty<bool> IsSelected => _isSelected.Value;

        public ReadOnlyReactiveProperty<int> Score => _score.Value;

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
