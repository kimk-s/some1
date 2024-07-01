using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IRankingFront
    {
        ReadOnlyReactiveProperty<Ranking> Ranking { get; }
        ReadOnlyReactiveProperty<bool> IsMine { get; }
    }
}
