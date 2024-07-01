using System;

namespace Some1.Play.Info
{
    public sealed class EnergyReachedInfo
    {
        public EnergyReachedInfo(EnergyId id, float normalizedValue, EnergyReachedOperator @operator, int delay)
        {
            if (normalizedValue < 0 || normalizedValue > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(normalizedValue));
            }

            if (delay < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(delay));
            }

            Id = id;
            NormalizedValue = normalizedValue;
            Operator = @operator;
            Delay = delay;
        }

        public EnergyId Id { get; }

        public float NormalizedValue { get; }

        public EnergyReachedOperator Operator { get; }

        public int Delay { get; }
    }

    public enum EnergyReachedOperator
    {
        Equals,
        Less,
        LessOrEquals,
        Greater,
        GreaterOrEquals,
    }
}
