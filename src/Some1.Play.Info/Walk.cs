using System;

namespace Some1.Play.Info
{
    public readonly struct Walk : IEquatable<Walk>
    {
        public Walk(bool isOn, float rotation)
        {
            IsOn = isOn;
            Rotation = rotation;
        }

        public bool IsOn { get; }

        public float Rotation { get; }

        public override bool Equals(object? obj) => obj is Walk other && Equals(other);

        public bool Equals(Walk other) => IsOn == other.IsOn && Rotation == other.Rotation;

        public override int GetHashCode() => HashCode.Combine(IsOn, Rotation);

        public override string ToString() => $"<{IsOn} {Rotation}>";

        public static bool operator ==(Walk left, Walk right) => left.Equals(right);

        public static bool operator !=(Walk left, Walk right) => !(left == right);
    }
}
