using System;

namespace Some1.Play.Info
{
    public readonly struct CharacterEnergyId : IEquatable<CharacterEnergyId>
    {
        public CharacterEnergyId(CharacterId character, EnergyId energy)
        {
            Character = character;
            Energy = energy;
        }

        public CharacterId Character { get; }

        public EnergyId Energy { get; }

        public override bool Equals(object? obj) => obj is CharacterEnergyId other && Equals(other);

        public bool Equals(CharacterEnergyId other) => Character == other.Character && Energy == other.Energy;

        public override int GetHashCode() => HashCode.Combine(Character, Energy);

        public override string ToString() => $"<{Character} {Energy}>";

        public static bool operator ==(CharacterEnergyId left, CharacterEnergyId right) => left.Equals(right);

        public static bool operator !=(CharacterEnergyId left, CharacterEnergyId right) => !(left == right);
    }
}
