using System;
using Some1.Play.Info;

namespace Some1.Play.Core
{
    public readonly struct Like : IEquatable<Like>
    {
        public Like(PlayerId playerId)
        {
            PlayerId = playerId;
        }

        public PlayerId PlayerId { get; }

        public override bool Equals(object? obj) => obj is Like other && Equals(other);

        public bool Equals(Like other) => PlayerId.Equals(other.PlayerId);

        public override int GetHashCode() => HashCode.Combine(PlayerId);

        public override string ToString() => PlayerId.ToString();

        public static bool operator ==(Like left, Like right) => left.Equals(right);

        public static bool operator !=(Like left, Like right) => !(left == right);
    }
}
