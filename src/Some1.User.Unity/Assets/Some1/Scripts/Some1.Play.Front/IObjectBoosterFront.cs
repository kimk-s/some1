using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IObjectBoosterFront
    {
        BoosterId Id { get; }
        ReadOnlyReactiveProperty<int> Number { get; }
        ReadOnlyReactiveProperty<float> Cycles { get; }
        ReadOnlyReactiveProperty<float> NormalizedConsumingDelay { get; }
        ReadOnlyReactiveProperty<float> Time { get; }
    }
}
