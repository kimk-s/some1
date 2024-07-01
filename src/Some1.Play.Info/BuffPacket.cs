using System;

namespace Some1.Play.Info
{
    public readonly struct BuffPacket : IEquatable<BuffPacket>
    {
        public BuffPacket(BuffId id, SkinId skinId)
        {
            Id = id;
            SkinId = skinId;
        }

        public BuffId Id { get; }

        public SkinId SkinId { get; }

        public override bool Equals(object? obj) => obj is BuffPacket other && Equals(other);

        public bool Equals(BuffPacket other) => Id == other.Id && SkinId == other.SkinId;

        public override int GetHashCode() => HashCode.Combine(Id, SkinId);

        public override string ToString() => $"<{Id} {SkinId}>";

        public static bool operator ==(BuffPacket left, BuffPacket right) => left.Equals(right);

        public static bool operator !=(BuffPacket left, BuffPacket right) => !(left == right);
    }
}
