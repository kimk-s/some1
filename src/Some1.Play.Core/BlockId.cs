using System;
using System.Drawing;

namespace Some1.Play.Core
{
    public readonly struct BlockId : IEquatable<BlockId>
    {
        public BlockId(int x, int y) : this(new(x, y))
        {
        }

        public BlockId(Point point)
        {
            if (point.X < 0 || point.Y < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(point));
            }

            Point = point;
        }

        public Point Point { get; }

        public int X => Point.X;

        public int Y => Point.Y;

        public override bool Equals(object? obj) => obj is BlockId other && Equals(other);

        public bool Equals(BlockId other) => Point == other.Point;

        public override int GetHashCode() => Point.GetHashCode();

        public override string ToString() => Point.ToString();

        public static bool operator ==(BlockId left, BlockId right) => left.Equals(right);

        public static bool operator !=(BlockId left, BlockId right) => !(left == right);

        public static implicit operator Point(BlockId id) => id.Point;

        public static implicit operator BlockId(Point id) => new(id);
    }
}
