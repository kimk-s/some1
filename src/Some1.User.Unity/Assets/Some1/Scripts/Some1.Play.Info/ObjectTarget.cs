using System;

namespace Some1.Play.Info
{
    public readonly struct ObjectTarget : IEquatable<ObjectTarget>
    {
        public ObjectTarget(ObjectTargetInfo info, byte team) : this(info.Character, new(info.Team, team))
        {
        }

        public ObjectTarget(CharacterTypeTarget characterType, TeamTarget team)
        {
            CharacterType = characterType;
            Team = team;
        }

        public static ObjectTarget All => new(CharacterTypeTarget.All, TeamTarget.All);

        public static ObjectTarget None => new(CharacterTypeTarget.None, TeamTarget.None);

        public CharacterTypeTarget CharacterType { get; }

        public TeamTarget Team { get; }

        public override bool Equals(object? obj) => obj is ObjectTarget other && Equals(other);

        public bool Equals(ObjectTarget other) => CharacterType == other.CharacterType && Team == other.Team;

        public override int GetHashCode() => HashCode.Combine(CharacterType, Team);

        public override string ToString() => $"<{CharacterType}, {Team}>";

        public static bool operator ==(ObjectTarget left, ObjectTarget right) => left.Equals(right);

        public static bool operator !=(ObjectTarget left, ObjectTarget right) => !(left == right);
    }
}
