using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IPlayerSeasonFront
    {
        public SeasonId Id { get; }
        public SeasonType Type { get; }
        public ReadOnlyReactiveProperty<Leveling?> Star { get; }
    }
}
