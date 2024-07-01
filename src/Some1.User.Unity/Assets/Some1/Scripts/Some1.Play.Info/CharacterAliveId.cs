using System;

namespace Some1.Play.Info
{
    public readonly struct CharacterAliveId : IEquatable<CharacterAliveId>
    {
        public CharacterAliveId(CharacterId character, bool alive)
        {
            Character = character;
            Alive = alive;
        }

        public CharacterId Character { get; }

        public bool Alive { get; }

        public override bool Equals(object? obj) => obj is CharacterAliveId other && Equals(other);

        public bool Equals(CharacterAliveId other) => Character == other.Character && Alive == other.Alive;

        public override int GetHashCode() => HashCode.Combine(Character, Alive);

        public override string ToString() => $"<{Character} {Alive}>";

        public static bool operator ==(CharacterAliveId left, CharacterAliveId right) => left.Equals(right);

        public static bool operator !=(CharacterAliveId left, CharacterAliveId right) => !(left == right);
    }
}
