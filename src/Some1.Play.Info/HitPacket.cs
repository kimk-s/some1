using System;

namespace Some1.Play.Info
{
    public readonly struct HitPacket : IEquatable<HitPacket>
    {
        public HitPacket(HitId id, int value, int rootId, float angle)
        {
            Id = id;
            Value = value;
            RootId = rootId;
            Angle = angle;
        }

        public HitId Id { get; }

        public int Value { get; }

        public int RootId { get; }

        public float Angle { get; }

        public override bool Equals(object? obj) => obj is HitPacket other && Equals(other);

        public bool Equals(HitPacket other) => Id == other.Id && Value == other.Value && RootId == other.RootId && Angle == other.Angle;

        public override int GetHashCode() => HashCode.Combine(Id, Value, RootId, Angle);

        public override string ToString() => $"<{Id} {Value} {RootId} {Angle}>";

        public static bool operator ==(HitPacket left, HitPacket right) => left.Equals(right);

        public static bool operator !=(HitPacket left, HitPacket right) => !(left == right);
    }
}
