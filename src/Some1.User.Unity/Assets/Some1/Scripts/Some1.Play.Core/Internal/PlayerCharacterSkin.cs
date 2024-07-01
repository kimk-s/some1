using System.Buffers;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;
using R3;

namespace Some1.Play.Core.Internal
{
    internal sealed class PlayerCharacterSkin : IPlayerCharacterSkin, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly ReactiveProperty<bool> _isElected = new();
        private readonly ReactiveProperty<bool> _isSelected = new();
        private readonly ReactiveProperty<bool> _isRandomSelected = new();
        private readonly CharacterSkinInfo _info;
        private bool _isPremium;

        internal PlayerCharacterSkin(CharacterSkinInfo info)
        {
            _info = info;
            Id = _info.Id.Skin;
            _sync = new SyncArraySource(
                _isElected.ToUnmanagedParticleSource(),
                _isSelected.ToUnmanagedParticleSource(),
                _isRandomSelected.ToUnmanagedParticleSource());

            UpdateIsUnlocked();
        }

        public SkinId Id { get; }

        public bool IsUnlocked { get; private set; }

        public bool IsElected { get => _isElected.Value; internal set => _isElected.Value = value; }

        public bool IsSelected { get => _isSelected.Value; internal set => _isSelected.Value = value; }

        public bool IsRandomSelected { get => _isRandomSelected.Value; internal set => _isRandomSelected.Value = value; }

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal bool IsPremium
        {
            get => _isPremium;
            set
            {
                if (_isPremium == value)
                {
                    return;
                }
                _isPremium = value;
                UpdateIsUnlocked();
            }
        }

        public void ClearDirty()
        {
            _sync.ClearDirty();
        }

        public void Dispose()
        {
            _sync.Dispose();
        }

        public void Write<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, SyncMode mode) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
        {
            _sync.Write(ref writer, mode);
        }

        internal void Reset()
        {
            IsElected = false;
            IsSelected = false;
            IsRandomSelected = false;
            IsPremium = false;
        }

        private void UpdateIsUnlocked()
        {
            IsUnlocked = !_info.IsPremium || IsPremium;
        }
    }
}
