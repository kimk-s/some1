using System;

namespace Some1.Play.Info
{
    public readonly struct CharacterIdleId : IEquatable<CharacterIdleId>
    {
        public CharacterIdleId(CharacterId character, bool idle)
        {
            Character = character;
            Idle = idle;
        }

        public CharacterId Character { get; }

        public bool Idle { get; }

        public override bool Equals(object? obj) => obj is CharacterIdleId other && Equals(other);

        public bool Equals(CharacterIdleId other) => Character == other.Character && Idle == other.Idle;

        public override int GetHashCode() => HashCode.Combine(Character, Idle);

        public override string ToString() => $"<{Character} {Idle}>";

        public static bool operator ==(CharacterIdleId left, CharacterIdleId right) => left.Equals(right);

        public static bool operator !=(CharacterIdleId left, CharacterIdleId right) => !(left == right);
    }
}
