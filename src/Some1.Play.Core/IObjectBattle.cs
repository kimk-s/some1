using R3;

namespace Some1.Play.Core
{
    public interface IObjectBattle
    {
        ReadOnlyReactiveProperty<bool?> Battle { get; }
    }
}
