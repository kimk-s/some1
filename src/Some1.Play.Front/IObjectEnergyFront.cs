using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IObjectEnergyFront
    {
        EnergyId Id { get; }
        ReadOnlyReactiveProperty<int> MaxValue { get; }
        ReadOnlyReactiveProperty<int> Value { get; }
        ReadOnlyReactiveProperty<float> NormalizedValue { get; }
    }
}
