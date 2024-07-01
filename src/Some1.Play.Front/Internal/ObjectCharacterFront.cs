using System.Collections.Generic;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class ObjectCharacterFront : IObjectCharacterFront, ISyncDestination
    {
        private readonly NullableUnmanagedParticleDestination<CharacterId> _id = new();

        public ObjectCharacterFront(CharacterInfoGroup infos)
        {
            Info = Id.Select(x => x is null ? null : infos.ById.GetValueOrDefault(x.Value))
                .ToReadOnlyReactiveProperty();
        }

        public ReadOnlyReactiveProperty<CharacterId?> Id => _id.Value;

        public ReadOnlyReactiveProperty<CharacterInfo?> Info { get; }

        public ReadOnlyReactiveProperty<bool> IsDefault => _id.IsDefault;

        public void Dispose()
        {
            _id.Dispose();
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            _id.Read(ref reader, mode);
        }

        public void Reset()
        {
            _id.Reset();
        }
    }
}
