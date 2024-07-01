using System;

namespace Some1.Play.Info
{
    public readonly struct CharacterStatId : IEquatable<CharacterStatId>
    {
        public CharacterStatId(CharacterId character, StatId stat)
        {
            Character = character;
            Stat = stat;
        }

        public CharacterId Character { get; }

        public StatId Stat { get; }

        public override bool Equals(object? obj) => obj is CharacterStatId other && Equals(other);

        public bool Equals(CharacterStatId other) => Character == other.Character && Stat == other.Stat;

        public override int GetHashCode() => HashCode.Combine(Character, Stat);

        public override string ToString() => $"<{Character} {Stat}>";

        public static bool operator ==(CharacterStatId left, CharacterStatId right) => left.Equals(right);

        public static bool operator !=(CharacterStatId left, CharacterStatId right) => !(left == right);
    }
}
