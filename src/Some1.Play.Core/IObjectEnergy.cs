using Some1.Play.Info;
using R3;

namespace Some1.Play.Core
{
    public interface IObjectEnergy
    {
        EnergyId Id { get; }
        ReadOnlyReactiveProperty<int> MaxValue { get; }
        ReadOnlyReactiveProperty<int> Value { get; }
        float NormalizedValue { get; }
    }

    public static class AbstractionObjectEnergyExtensions
    {
        public static bool IsFilledUp(this IObjectEnergy energy)
        {
            return energy.MaxValue.CurrentValue > 0 && energy.MaxValue.CurrentValue == energy.Value.CurrentValue;
        }

        public static bool IsCleared(this IObjectEnergy energy)
        {
            return energy.Value.CurrentValue == 0;
        }
    }
}
