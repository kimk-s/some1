using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerCharacterSkinFront : IPlayerCharacterSkinFront, ISyncDestination
    {
        private readonly SyncArrayDestination _sync;
        private readonly UnmanagedParticleDestination<bool> _isElected = new();
        private readonly UnmanagedParticleDestination<bool> _isSelected = new();
        private readonly UnmanagedParticleDestination<bool> _isRandomSelected = new();

        internal PlayerCharacterSkinFront(
            CharacterSkinInfo info,
            IPlayerPremiumFront premium)
        {
            Id = info.Id;
            IsPremium = info.IsPremium;
            IsUnlocked = premium.IsPremium.Select(x => x || !IsPremium).ToReadOnlyReactiveProperty();
            _sync = new SyncArrayDestination(
                _isElected,
                _isSelected,
                _isRandomSelected);
        }

        public CharacterSkinId Id { get; }

        public bool IsPremium { get; }

        public ReadOnlyReactiveProperty<bool> IsUnlocked { get; }

        public ReadOnlyReactiveProperty<bool> IsElected => _isElected.Value;

        public ReadOnlyReactiveProperty<bool> IsSelected => _isSelected.Value;

        public ReadOnlyReactiveProperty<bool> IsRandomSelected => _isRandomSelected.Value;

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
