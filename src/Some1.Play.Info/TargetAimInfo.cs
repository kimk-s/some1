using System;

namespace Some1.Play.Info
{
    public readonly struct TargetAimInfo : IEquatable<TargetAimInfo>
    {
        public TargetAimInfo(Mixable<float> rotation, Mixable<float> length)
        {
            Rotation = rotation;
            Length = length;
        }

        public Mixable<float> Rotation { get; }

        public Mixable<float> Length { get; }

        public override bool Equals(object? obj) => obj is TargetAimInfo other && Equals(other);

        public bool Equals(TargetAimInfo other) => Rotation.Equals(other.Rotation) && Length.Equals(other.Length);

        public override int GetHashCode() => HashCode.Combine(Rotation, Length);

        public override string ToString() => $"<{Rotation} {Length}>";

        public static bool operator ==(TargetAimInfo left, TargetAimInfo right) => left.Equals(right);

        public static bool operator !=(TargetAimInfo left, TargetAimInfo right) => !(left == right);
    }
}
