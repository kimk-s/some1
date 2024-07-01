using Some1.Play.Info;
using R3;

namespace Some1.Play.Core
{
    public interface IObjectTrait
    {
        Trait Value { get; }
        ReadOnlyReactiveProperty<Trait> Next { get; }
        ReadOnlyReactiveProperty<Trait> AfterNext { get; }
    }
}
