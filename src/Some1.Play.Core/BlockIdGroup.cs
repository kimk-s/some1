using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Some1.Play.Core
{
    public readonly struct BlockIdGroup : IEquatable<BlockIdGroup>, IEnumerable<BlockId>
    {
        public BlockIdGroup(int x, int y, int width, int height) : this(new(x, y, width, height))
        {
        }

        public BlockIdGroup(Point location, Size size) : this(new(location, size))
        {
        }

        public BlockIdGroup(Rectangle rectangle)
        {
            if (rectangle.X < 0 || rectangle.Y < 0 || rectangle.Width < 0 || rectangle.Height < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rectangle));
            }

            Rectangle = rectangle;
        }

        public static BlockIdGroup Empty { get; } = new(Rectangle.Empty);

        public Rectangle Rectangle { get; }

        public Point Location => Rectangle.Location;

        public Size Size => Rectangle.Size;

        public int X => Rectangle.X;

        public int Y => Rectangle.Y;

        public int Width => Rectangle.Width;

        public int Height => Rectangle.Height;

        public int Left => Rectangle.Left;

        public int Top => Rectangle.Top;

        public int Right => Rectangle.Right;

        public int Bottom => Rectangle.Bottom;

        public bool IsEmpty => Rectangle.IsEmpty;

        public readonly bool Contains(BlockId id) => Contains(id.X, id.Y);

        public readonly bool Contains(int x, int y) => Rectangle.Contains(x, y);

        public readonly bool Contains(Point pt) => Rectangle.Contains(pt);

        public readonly bool Contains(Rectangle rect) => Rectangle.Contains(rect);

        public Enumerator GetEnumerator() => new(this);

        IEnumerator<BlockId> IEnumerable<BlockId>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override bool Equals(object? obj) => obj is BlockIdGroup other && Equals(other);

        public bool Equals(BlockIdGroup other) => Rectangle.Equals(other.Rectangle);

        public override int GetHashCode() => Rectangle.GetHashCode();

        public override string ToString() => Rectangle.ToString();

        public static bool operator ==(BlockIdGroup left, BlockIdGroup right) => left.Equals(right);

        public static bool operator !=(BlockIdGroup left, BlockIdGroup right) => !(left == right);

        public static implicit operator BlockIdGroup(Rectangle rectangle) => new(rectangle);

        public struct Enumerator : IEnumerator<BlockId>, IEnumerator
        {
            private readonly BlockIdGroup _group;
            private Point _index;
            private BlockId _current;
            private int _nForDebug;

            public Enumerator(BlockIdGroup group)
            {
                _group = group;
                _index = new();
                _current = new();

                _nForDebug = 0;
            }

            public readonly void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (++_nForDebug > 100_000)
                {
                    throw new Exception("nForDebug > 100_000");
                }

                if ((uint)_index.Y < (uint)_group.Height)
                {
                    _current = new(_group.X + _index.X, _group.Y + _index.Y);
                    _index.X++;
                    if (_index.X == _group.Width)
                    {
                        _index.X = 0;
                        _index.Y++;
                    }
                    return true;
                }
                return MoveNextRare();
            }

            private bool MoveNextRare()
            {
                _index = new(0, _group.Height + 1);
                _current = default;
                return false;
            }

            public readonly BlockId Current => _current;

            readonly object IEnumerator.Current
            {
                get
                {
                    if (_index == default || _index.X == _group.Height + 1)
                    {
                        throw new InvalidOperationException();
                    }
                    return Current;
                }
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }
    }
}
