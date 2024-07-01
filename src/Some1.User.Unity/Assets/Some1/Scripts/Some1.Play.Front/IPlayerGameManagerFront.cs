using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IPlayerGameManagerFront
    {
        ReadOnlyReactiveProperty<GameManagerStatus?> Status { get; }
        ReadOnlyReactiveProperty<float> Cycles { get; }
    }
}
