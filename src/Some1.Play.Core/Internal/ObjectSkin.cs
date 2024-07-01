using System;
using System.Buffers;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using R3;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectSkin : IObjectSkin, ISyncSource
    {
        private readonly NullableUnmanagedParticleSource<SkinId> _sync;
        private readonly CharacterSkinInfoGroup _infos;
        private readonly ReactiveProperty<SkinId?> _id = new();
        private readonly ObjectProperties _properties;
        private CharacterId? _characterId;

        public ObjectSkin(CharacterSkinInfoGroup infos, ObjectProperties properties)
        {
            _infos = infos ?? throw new ArgumentNullException(nameof(infos));
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
            _sync = _id.ToNullableUnmanagedParticleSource();
        }

        public SkinId? Id { get => _id.Value; set => _id.Value = value; }

        public bool Value { get; private set; }

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal void Set(SkinId? id)
        {
            if (Id == id)
            {
                return;
            }
            Id = id;
            _properties.SkinId = id;
            UpdateValue();
        }

        internal void Set(CharacterId characterId)
        {
            if (_characterId == characterId)
            {
                return;
            }
            _characterId = characterId;
            UpdateValue();
        }

        internal void Reset()
        {
            Id = null;
            _characterId = null;
            Value = false;
        }

        private void UpdateValue()
        {
            Value = Id is not null
                && _characterId is not null
                && (_infos.ById.ContainsKey(new(_characterId.Value, Id.Value)) || _infos.ById.ContainsKey(new(_characterId.Value, SkinId.Skin0)));
        }

        public void ClearDirty()
        {
            _sync.ClearDirty();
        }

        public void Write<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, SyncMode mode) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
        {
            _sync.Write(ref writer, mode);
        }

        public void Dispose()
        {
            _sync.Dispose();
        }
    }
}
