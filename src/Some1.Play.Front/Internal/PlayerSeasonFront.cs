using System.Linq;
using R3;
using Some1.Play.Info;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerSeasonFront : IPlayerSeasonFront
    {
        public PlayerSeasonFront(
            SeasonInfo info,
            IPlayerCharacterGroupFront characters,
            IPlayerSpecialtyGroupFront specialties)
        {
            Id = info.Id;
            Type = info.Type;

            var seasonCharacters = characters.All.Values.Where(x => x.Season == info.Id);
            var seasonSpecialties = specialties.All.Values.Where(x => x.Season == info.Id);
            int maxStar = seasonCharacters.Count() * PlayConst.CharacterStarLeveling_MaxLevel + seasonSpecialties.Count() * PlayConst.SpecialtyStarLeveling_MaxLevel;
            var currentStar = Type == SeasonType.ComingSoon
                ? Observable.Return(0)
                : Observable
                    .CombineLatest(seasonCharacters.Select(x => x.Star.Select(x => x.Level)))
                    .Select(x => x.Sum())
                    .CombineLatest(
                        Observable
                            .CombineLatest(seasonSpecialties.Select(x => x.Star.Select(x => x.Level)))
                            .Select(x => x.Sum()),
                        (a, b) => a + b);
            Star = currentStar
                .Select(x => (Leveling?)new Leveling(x, 1, maxStar, LevelingMethod.Plain))
                .ToReadOnlyReactiveProperty();
        }

        public SeasonId Id { get; }

        public SeasonType Type { get; }

        public ReadOnlyReactiveProperty<Leveling?> Star { get; }
    }
}
