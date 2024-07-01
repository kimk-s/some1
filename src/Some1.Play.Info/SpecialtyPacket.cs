using System;

namespace Some1.Play.Info
{
    public readonly struct SpecialtyPacket : IEquatable<SpecialtyPacket>
    {
        public SpecialtyPacket(SpecialtyId id, int number, bool isPinned, int token)
        {
            Id = id;
            Number = number;
            IsPinned = isPinned;
            Token = token;
        }

        public SpecialtyId Id { get; }

        public int Number { get; }

        public bool IsPinned { get; }

        public int Token { get; }

        public override bool Equals(object? obj) => obj is SpecialtyPacket other && Equals(other);

        public bool Equals(SpecialtyPacket other) => Id == other.Id && Number == other.Number && IsPinned == other.IsPinned && Token == other.Token;

        public override int GetHashCode() => HashCode.Combine(Id, Number, IsPinned, Token);

        public override string ToString() => $"<{Id} {Number} {IsPinned} {Token}>";

        public static bool operator ==(SpecialtyPacket left, SpecialtyPacket right) => left.Equals(right);

        public static bool operator !=(SpecialtyPacket left, SpecialtyPacket right) => !(left == right);
    }
}
