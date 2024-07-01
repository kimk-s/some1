using System;

namespace Some1.Play.Core.Internal
{
    internal static class ObjectEnergyExtensions
    {
        internal static void SetValueRate(this ObjectEnergy energy, float rate = 1.0f)
        {
            rate = Math.Clamp(rate, 0, 1);
            energy.SetValue((int)(energy.MaxValue.CurrentValue * rate));
        }

        internal static void Clear(this ObjectEnergy energy)
        {
            energy.SetValue(0);
        }

        internal static bool CanConsume(this IObjectEnergy energy, int cost)
        {
            if (cost < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(cost));
            }

            return energy.Value.CurrentValue >= cost;
        }

        internal static void Consume(this ObjectEnergy energy, int cost)
        {
            if (cost < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(cost));
            }

            if (!energy.CanConsume(cost))
            {
                throw new InvalidOperationException();
            }

            energy.SetValue(energy.Value.CurrentValue - cost);
        }
    }
}
