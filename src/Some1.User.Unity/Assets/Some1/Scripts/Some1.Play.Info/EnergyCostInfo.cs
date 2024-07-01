using System;

namespace Some1.Play.Info
{
    public sealed class EnergyCostInfo
    {
        public EnergyCostInfo(EnergyId id, int value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            Id = id;
            Value = value;
        }

        public EnergyId Id { get; }

        public int Value { get; }
    }
}
