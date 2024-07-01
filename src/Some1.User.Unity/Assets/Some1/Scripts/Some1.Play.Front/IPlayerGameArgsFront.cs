using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IPlayerGameArgsFront
    {
        GameMode Id { get; }
        ReadOnlyReactiveProperty<bool> IsSelected { get; }
        ReadOnlyReactiveProperty<int> Score { get; }
    }
}
