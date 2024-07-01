using System;
using System.Numerics;

namespace Some1.Play.Info
{
    public readonly struct Aim : IEquatable<Aim>
    {
        public Aim(Vector2 vector2) : this(Vector2Helper.Angle(vector2), vector2.Length())
        {
        }

        public Aim(float rotation, float length)
        {
            Rotation = rotation;
            Length = length;
        }

        public static Aim Zero { get; } = new();

        public static Aim Auto { get; } = new(-1, 0);

        public float Rotation { get; }

        public float Length { get; }

        public bool IsAuto => Rotation < 0;

        public Vector2 ToVector2() => Vector2Helper.Normalize(Rotation) * Length;

        public override bool Equals(object? obj) => obj is Aim other && Equals(other);

        public bool Equals(Aim other) => Rotation == other.Rotation && Length == other.Length;

        public override int GetHashCode() => HashCode.Combine(Rotation, Length);

        public override string ToString() => $"<{Rotation} {Length}>";

        public static bool operator ==(Aim left, Aim right) => left.Equals(right);

        public static bool operator !=(Aim left, Aim right) => !(left == right);
    }
}
