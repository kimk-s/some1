using System;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal readonly struct ObjectStatic : IBlockItemStatic, IEquatable<ObjectStatic>
    {
        public ObjectStatic(int id, CharacterType characterType, byte team)
        {
            Id = id;
            CharacterType = characterType;
            Team = team;
        }

        public int Id { get; }

        public CharacterType CharacterType { get; }

        public byte Team { get; }

        public override bool Equals(object? obj) => obj is ObjectStatic other && Equals(other);

        public bool Equals(ObjectStatic other) => Id == other.Id && CharacterType == other.CharacterType && Team == other.Team;

        public override int GetHashCode() => HashCode.Combine(Id, CharacterType, Team);

        public static bool operator ==(ObjectStatic left, ObjectStatic right) => left.Equals(right);

        public static bool operator !=(ObjectStatic left, ObjectStatic right) => !(left == right);

        public bool IsTarget(ObjectTarget target) => target.CharacterType.IsMatch(CharacterType) && target.Team.IsMatch(Team);
    }
}
