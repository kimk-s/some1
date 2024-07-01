using R3;

namespace Some1.Play.Front
{
    public interface IObjectBattleFront
    {
        ReadOnlyReactiveProperty<bool?> Battle { get; }
    }
}
