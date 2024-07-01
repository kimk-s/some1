using System;

namespace Some1.Play.Info
{
    public readonly struct Title : IEquatable<Title>
    {
        public Title(int star, PlayerId playerId, byte like, Medal medal)
        {
            Star = star;
            PlayerId = playerId;
            Like = like;
            Medal = medal;
        }

        public static Title Empty => default;

        public int Star { get; }

        public PlayerId PlayerId { get; }

        public byte Like { get; }

        public Medal Medal { get; }

        public override bool Equals(object? obj) => obj is Title other && Equals(other);

        public bool Equals(Title other)
            => Star.Equals(other.Star)
            && PlayerId.Equals(other.PlayerId)
            && Like == other.Like
            && Medal == other.Medal;

        public override int GetHashCode() => HashCode.Combine(Star, PlayerId, Like, Medal);

        public override string ToString() => $"<{Star} {PlayerId} {Like} {Medal}>";

        public static bool operator ==(Title left, Title right) => left.Equals(right);

        public static bool operator !=(Title left, Title right) => !(left == right);
    }
}
