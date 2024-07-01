using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IPlayerGameFront
    {
        ReadOnlyReactiveProperty<Game?> Game { get; }
        ReadOnlyReactiveProperty<float> Cycles { get; }
    }
}
