using System;
using System.Numerics;

namespace Some1.Play.Info
{
    public readonly struct NonPlayerRectTransform
    {
        public NonPlayerRectTransform(Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 position, Vector2 size)
        {
            if (!IsInZeroAndOne(anchorMin))
            {
                throw new ArgumentOutOfRangeException(nameof(anchorMin));
            }

            if (!IsInZeroAndOne(anchorMax))
            {
                throw new ArgumentOutOfRangeException(nameof(anchorMax));
            }

            if (!IsInZeroAndOne(pivot))
            {
                throw new ArgumentOutOfRangeException(nameof(pivot));
            }

            if (!IsPositive(position))
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            if (!IsPositive(size))
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            if (anchorMin.X > anchorMax.X || anchorMin.Y > anchorMax.Y)
            {
                throw new InvalidOperationException();
            }

            AnchorMin = anchorMin;
            AnchorMax = anchorMax;
            Pivot = pivot;
            Position = position;
            Size = size;
        }

        public Vector2 AnchorMin { get; }
        public Vector2 AnchorMax { get; }
        public Vector2 Pivot { get; }
        public Vector2 Position { get; }
        public Vector2 Size { get; }

        public bool Stretch => AnchorMin != AnchorMax;

        private static bool IsInZeroAndOne(Vector2 vector) => vector.X >= 0 && vector.X <= 1 && vector.Y >= 0 && vector.Y <= 1;

        private static bool IsPositive(Vector2 vector) => vector.X >= 0 || vector.Y >= 0;
    }
}
