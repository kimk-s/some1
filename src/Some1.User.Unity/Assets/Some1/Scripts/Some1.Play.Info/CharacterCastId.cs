using System;

namespace Some1.Play.Info
{
    public readonly struct CharacterCastId : IEquatable<CharacterCastId>
    {
        public CharacterCastId(CharacterId character, CastId cast)
        {
            Character = character;
            Cast = cast;
        }

        public CharacterId Character { get; }

        public CastId Cast { get; }

        public override bool Equals(object? obj) => obj is CharacterCastId other && Equals(other);

        public bool Equals(CharacterCastId other) => Character == other.Character && Cast == other.Cast;

        public override int GetHashCode() => HashCode.Combine(Character, Cast);

        public override string ToString() => $"<{Character} {Cast}>";

        public static bool operator ==(CharacterCastId left, CharacterCastId right) => left.Equals(right);

        public static bool operator !=(CharacterCastId left, CharacterCastId right) => !(left == right);
    }
}
