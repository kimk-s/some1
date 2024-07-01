using System;
using System.Drawing;
using System.Numerics;

namespace Some1.Play.Info
{
    public struct Area : IEquatable<Area>
    {
        public const float ToleranceInflation = 0.0005f;
        private RectangleF _data;

        internal Area(AreaType type, RectangleF data)
        {
            Type = type;
            _data = data;
        }

        public static Area Empty { get; }

        public AreaType Type { get; set; }

        public readonly RectangleF Data => _data;

        public Vector2 Position
        {
            readonly get => Location.ToVector2() + Size.ToVector2() * 0.5f;
            set => Location = (value - Size.ToVector2() * 0.5f).ToPointF();
        }

        public PointF Location
        {
            readonly get => Data.Location;
            set => _data.Location = value;
        }

        public SizeF Size
        {
            readonly get => Data.Size;
            set
            {
                if (Type != AreaType.Rectangle && value.Width != value.Height)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                _data.Size = value;
            }
        }

        public readonly float Left => Data.Left;

        public readonly float Top => Data.Top;

        public readonly float Right => Data.Right;

        public readonly float Bottom => Data.Bottom;

        public static Area Rectangle(Vector2 position, float size) => Rectangle(position, new SizeF(size, size));

        public static Area Rectangle(PointF location, float size) => Rectangle(location, new SizeF(size, size));

        public static Area Rectangle(Vector2 position, SizeF size) => Rectangle((position - size.ToVector2() * 0.5f).ToPointF(), size);

        public static Area Rectangle(PointF location, SizeF size) => new RectangleF(location, size);

        public static Area Circle(Vector2 position, float diameter) => Circle((position - new Vector2(diameter * 0.5f, diameter * 0.5f)).ToPointF(), diameter);

        public static Area Circle(PointF location, float diameter) => new(AreaType.Circle, new(location, new(diameter, diameter)));

        public static Area From(CharacterAreaInfo info, Vector2 position) => Create(info.Type, position, info.Size);

        public static Area From(AreaInfo info, Area area) => Create(
            info.Type == AreaType.Rectangle || area.Type == AreaType.Rectangle ? AreaType.Rectangle : AreaType.Circle,
            info.Position.ToVector2() + area.Position,
            info.Size + area.Size);

        public static Area From(AreaInfo info, Vector2 position, float scale = 1) => Create(
            info.Type,
            info.Position.ToVector2() + position,
            info.Size * scale);

        private static Area Create(AreaType type, Vector2 position, float size) => type switch
        {
            AreaType.Rectangle => Rectangle(position, size),
            AreaType.Circle => Circle(position, size),
            _ => throw new NotImplementedException()
        };

        private static Area Create(AreaType type, Vector2 position, SizeF size) => type switch
        {
            AreaType.Rectangle => Rectangle(position, size),
            AreaType.Circle => Circle(position, size.Width),
            _ => throw new NotImplementedException()
        };

        public readonly bool Contains(Area area)
        {
            if (Type != AreaType.Rectangle)
            {
                throw new InvalidOperationException();
            }

            return Data.Contains(area.Data);
        }

        public readonly bool Contains(PointF point)
        {
            if (Type != AreaType.Rectangle)
            {
                throw new InvalidOperationException();
            }

            return Data.Contains(point);
        }

        public readonly bool Contains(Vector2 position)
        {
            if (Type != AreaType.Rectangle)
            {
                throw new InvalidOperationException();
            }

            return Data.Contains(position.X, position.Y);
        }

        public void Intersect(Area area) => _data.Intersect(area.Data);

        public readonly bool IntersectsWith(Area area)
        {
            if (area.Type == AreaType.Rectangle && Type == AreaType.Rectangle)
            {
                return IntersectsWithRectangleAndRectangle(area.Data, Data);
            }
            else if (area.Type == AreaType.Rectangle && Type == AreaType.Circle)
            {
                return IntersectsWithRectangleAndCircle(area.Data, Data);
            }
            else if (area.Type == AreaType.Circle && Type == AreaType.Rectangle)
            {
                return IntersectsWithRectangleAndCircle(Data, area.Data);
            }
            else
            {
                return IntersectsWithCircleAndCircle(area.Data, Data);
            }
        }

        public void RoundLocation()
        {
            Location = Point.Round(Location);
        }

        public override readonly bool Equals(object? obj) => obj is Area other && Equals(other);

        public readonly bool Equals(Area other) => Type == other.Type && Data.Equals(other.Data);

        public override readonly int GetHashCode() => HashCode.Combine(Type, Data);

        public override readonly string ToString() => $"<{Type} {Data}>";

        public static implicit operator Area(RectangleF rectangle) => new(AreaType.Rectangle, rectangle);

        public static explicit operator RectangleF(Area area)
        {
            if (area.Type != AreaType.Rectangle)
            {
                throw new InvalidOperationException();
            }

            return area.Data;
        }

        public static bool operator ==(Area left, Area right) => left.Equals(right);

        public static bool operator !=(Area left, Area right) => !(left == right);

        private static bool IntersectsWithRectangleAndRectangle(RectangleF rect1, RectangleF rect2)
        {
            rect1.Inflate(rect1.Size * -ToleranceInflation);

            return rect1.IntersectsWith(rect2);
        }

        private static bool IntersectsWithRectangleAndCircle(RectangleF rect, RectangleF circle)
        {
            rect.Inflate(rect.Size * -ToleranceInflation);

            var circleCenter = circle.Center();
            var circleRadius = circle.Width * 0.5f;

            var closestX = Math.Clamp(circleCenter.X, rect.Left, rect.Right);
            var closestY = Math.Clamp(circleCenter.Y, rect.Top, rect.Bottom);

            var distanceX = circleCenter.X - closestX;
            var distanceY = circleCenter.Y - closestY;

            var distanceSquared = MathF.Pow(distanceX, 2) + MathF.Pow(distanceY, 2);
            return distanceSquared < MathF.Pow(circleRadius, 2);
        }

        private static bool IntersectsWithCircleAndCircle(RectangleF circle1, RectangleF circle2)
        {
            circle1.Inflate(circle1.Size * -ToleranceInflation);

            var distanceX = circle1.X - circle2.X;
            var distanceY = circle1.Y - circle2.Y;

            var distanceSquared = MathF.Pow(distanceX, 2) + MathF.Pow(distanceY, 2);
            return distanceSquared < MathF.Pow(circle1.Width / 2 + circle2.Width / 2, 2);
        }
    }
}
