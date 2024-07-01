using System;

namespace Some1.Play.Info
{
    public readonly struct Shift : IEquatable<Shift>
    {
        public Shift(ShiftId id, float rotation, float distance, float time, float speed)
        {
            Id = id;
            Rotation = rotation;
            Distance = distance;
            Time = time;
            Speed = speed;
        }

        public ShiftId Id { get; }

        public float Rotation { get; }

        public float Distance { get; }

        public float Time { get; }

        public float Speed { get; }

        public float GetTransformRotation() => Id.GetTransformRotation(Rotation);

        public override string ToString() => $"<{Id} {Rotation} {Distance} {Time} {Speed}>";

        public override bool Equals(object? obj) => obj is Shift other && Equals(other);

        public bool Equals(Shift other)
            => Id == other.Id &&
            Rotation == other.Rotation &&
            Distance == other.Distance &&
            Time == other.Time &&
            Speed == other.Speed;

        public override int GetHashCode() => HashCode.Combine(Id, Rotation, Distance, Time, Speed);

        public static bool operator ==(Shift left, Shift right) => left.Equals(right);

        public static bool operator !=(Shift left, Shift right) => !(left == right);
    }
}
