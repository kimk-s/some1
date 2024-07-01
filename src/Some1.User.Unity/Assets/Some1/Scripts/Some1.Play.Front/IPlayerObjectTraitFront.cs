using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IPlayerObjectTraitFront
    {
        ReadOnlyReactiveProperty<Trait> Next { get; }
        ReadOnlyReactiveProperty<Trait> AfterNext { get; }
    }
}
