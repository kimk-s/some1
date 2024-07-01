using System;

namespace Some1.Play.Core
{
    public readonly struct BirthId : IEquatable<BirthId>
    {
        public BirthId(int parentId, int frameCount)
        {
            if (parentId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(parentId));
            }

            if (frameCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(frameCount));
            }

            ParentId = parentId;
            FrameCount = frameCount;
        }

        public int ParentId { get; }

        public int FrameCount { get; }

        public override bool Equals(object? obj) => obj is BirthId other && Equals(other);

        public bool Equals(BirthId other) => ParentId == other.ParentId && FrameCount == other.FrameCount;

        public override int GetHashCode() => HashCode.Combine(ParentId, FrameCount);

        public override string ToString() => $"<{ParentId} {FrameCount}>";

        public static bool operator ==(BirthId left, BirthId right) => left.Equals(right);

        public static bool operator !=(BirthId left, BirthId right) => !(left == right);
    }
}
