using R3;
using Some1.Play.Info;

namespace Some1.Play.Front
{
    public interface IPlayerTitleFront
    {
        int MaxStar { get; }
        ReadOnlyReactiveProperty<Leveling> StarGrade { get; }
        ReadOnlyReactiveProperty<PlayerId> PlayerId { get; }
        ReadOnlyReactiveProperty<Leveling> Like { get; }
        ReadOnlyReactiveProperty<Medal> Medal { get; }
    }
}
