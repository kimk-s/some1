using System;

namespace Some1.Play.Info
{
    public sealed class CharacterEnergyInfo
    {
        public CharacterEnergyInfo(CharacterEnergyId id, int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            Id = id;
            Value = value;
        }

        public CharacterEnergyId Id { get; }

        public int Value { get; }
    }
}
