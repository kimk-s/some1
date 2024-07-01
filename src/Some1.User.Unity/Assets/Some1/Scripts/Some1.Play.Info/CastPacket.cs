using System;

namespace Some1.Play.Info
{
    public readonly struct CastPacket : IEquatable<CastPacket>
    {
        public CastPacket(CastId id, Aim aim)
        {
            Id = id;
            Aim = aim;
        }

        public CastId Id { get; }

        public Aim Aim { get; }

        public override bool Equals(object? obj) => obj is CastPacket other && Equals(other);

        public bool Equals(CastPacket other) => Id == other.Id && Aim.Equals(other.Aim);

        public override int GetHashCode() => HashCode.Combine(Id, Aim);

        public override string ToString() => $"<{Id} {Aim}>";

        public static bool operator ==(CastPacket left, CastPacket right) => left.Equals(right);

        public static bool operator !=(CastPacket left, CastPacket right) => !(left == right);
    }
}
