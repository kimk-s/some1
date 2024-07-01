using System.Collections.Generic;
using System.Linq;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerObjectCastFront : IPlayerObjectCastFront, ISyncDestination, ISyncPolatable
    {
        private readonly SyncArrayDestination _sync;

        internal PlayerObjectCastFront(
            ISyncTime syncFrame,
            CharacterCastInfoGroup infos,
            ReadOnlyReactiveProperty<CharacterId?> characterId)
        {
            var items = EnumForUnity.GetValues<CastId>()
                .Select(x => new PlayerObjectCastItemFront(
                    syncFrame,
                    x,
                    infos,
                    characterId))
                .ToDictionary(x => x.Id);
            _sync = new(items.Values.OrderBy(x => x.Id));
            Items = items.Values.Select(x => (IPlayerObjectCastItemFront)x).ToDictionary(x => x.Id);
        }

        public IReadOnlyDictionary<CastId, IPlayerObjectCastItemFront> Items { get; }

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        public void Dispose()
        {
            _sync.Dispose();
        }

        public void Extrapolate()
        {
            _sync.Extrapolate();
        }

        public void Interpolate(float time)
        {
            _sync.Interpolate(time);
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
