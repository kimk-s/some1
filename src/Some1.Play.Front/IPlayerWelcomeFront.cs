using R3;

namespace Some1.Play.Front
{
    public interface IPlayerWelcomeFront
    {
        ReadOnlyReactiveProperty<bool> Welcome { get; }
    }
}
