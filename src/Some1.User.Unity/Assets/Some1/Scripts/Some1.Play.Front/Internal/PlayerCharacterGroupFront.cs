using System;
using System.Collections.Generic;
using System.Linq;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerCharacterGroupFront : IPlayerCharacterGroupFront, ISyncDestination
    {
        private readonly SyncArrayDestination _sync;
        private readonly Dictionary<CharacterId, PlayerCharacterFront> _all;
        private readonly UnmanagedParticleDestination<DateTime> _pickTime = new();

        internal PlayerCharacterGroupFront(
            CharacterInfoGroup characterInfos,
            CharacterSkillInfoGroup skillInfos,
            CharacterSkillPropInfoGroup skillPropInfos,
            CharacterEnergyInfoGroup characterEnergyInfos,
            CharacterSkinInfoGroup characterSkinInfos,
            IPlayerPremiumFront premium,
            ITimeFront time)
        {
            _all = characterInfos.WherePlayerById.Values
                .Select(x => new PlayerCharacterFront(
                    x,
                    skillInfos,
                    skillPropInfos,
                    characterEnergyInfos,
                    characterSkinInfos,
                    premium,
                    time))
                .ToDictionary(x => x.Id);
            All = _all.Values
                .Select(x => (IPlayerCharacterFront)x)
                .ToDictionary(x => x.Id);

            Selected = Observable
                .Merge(All.Values.Select(x => x.IsSelected.Where(x => x).Select(_ => x)))!
                .ToReadOnlyReactiveProperty();

            PickTimeRemained = PickTime
                .CombineLatest(
                    time.UtcNow,
                    (pickTime, utcNow) => pickTime == DateTime.MinValue
                        ? TimeSpan.Zero
                        : pickTime - utcNow)
                .ToReadOnlyReactiveProperty();

            int maxStar = All.Count * PlayConst.CharacterStarLeveling_MaxLevel;
            var currentStar = Observable
                .CombineLatest(All.Values.Select(x => x.Star.Select(x => x.Level)))
                .Select(x => x.Sum());
            Star = currentStar
                .Select(x => new Leveling(x, 1, maxStar, LevelingMethod.Plain))
                .ToReadOnlyReactiveProperty();

            _sync = new SyncArrayDestination(
                new SyncArrayDestination(_all.Values.OrderBy(x => x.Id)),
                _pickTime);
        }

        public IReadOnlyDictionary<CharacterId, IPlayerCharacterFront> All { get; }

        public ReadOnlyReactiveProperty<IPlayerCharacterFront?> Selected { get; }

        public ReadOnlyReactiveProperty<DateTime> PickTime => _pickTime.Value;

        public ReadOnlyReactiveProperty<TimeSpan> PickTimeRemained { get; }

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
