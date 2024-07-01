using System.Collections.Generic;
using System.Linq;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerSpecialtyGroupFront : IPlayerSpecialtyGroupFront, ISyncDestination
    {
        private readonly SyncArrayDestination _sync;
        private readonly Dictionary<SpecialtyId, PlayerSpecialtyFront> _all;

        internal PlayerSpecialtyGroupFront(SpecialtyInfoGroup infos, ITimeFront time)
        {
            _all = infos.ById.Values
                .Select(x => new PlayerSpecialtyFront(x, time))
                .ToDictionary(x => x.Id);
            All = _all.Values
                .Select(x => (IPlayerSpecialtyFront)x)
                .ToDictionary(x => x.Id);

            int maxStar = All.Count * PlayConst.SpecialtyStarLeveling_MaxLevel;
            var currentStar = Observable
                .CombineLatest(All.Values.Select(x => x.Star.Select(x => x.Level)))
                .Select(x => x.Sum());
            Star = currentStar
                .Select(x => new Leveling(x, 1, maxStar, LevelingMethod.Plain))
                .ToReadOnlyReactiveProperty();

            _sync = new SyncArrayDestination(_all.Values.OrderBy(x => x.Id));
        }

        public IReadOnlyDictionary<SpecialtyId, IPlayerSpecialtyFront> All { get; }

        public ReadOnlyReactiveProperty<Leveling> Star { get; }

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
