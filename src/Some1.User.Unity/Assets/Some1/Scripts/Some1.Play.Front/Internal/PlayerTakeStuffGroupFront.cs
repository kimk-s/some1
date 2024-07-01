using MemoryPack;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerTakeStuffGroupFront : IPlayerTakeStuffGroupFront, ISyncDestination
    {
        private readonly UnmanagedParticleDestination<int> _comboScore = new();

        public ReadOnlyReactiveProperty<int> ComboScore => _comboScore.Value;

        public ReadOnlyReactiveProperty<bool> IsDefault => _comboScore.IsDefault;

        public void Dispose()
        {
            _comboScore.Dispose();
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            _comboScore.Read(ref reader, mode);
        }

        public void Reset()
        {
            _comboScore.Reset();
        }
    }
}
