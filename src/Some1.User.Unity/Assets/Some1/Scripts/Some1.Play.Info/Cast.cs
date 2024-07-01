using System;

namespace Some1.Play.Info
{
    public readonly struct Cast
    {
        public Cast(CastId id, short token, bool isOn, Aim aim)
        {
            Id = id;
            Token = token;
            IsOn = isOn;
            Aim = aim;
        }

        public CastId Id { get; }

        public short Token { get; }

        public bool IsOn { get; }

        public Aim Aim { get; }

        public override bool Equals(object? obj) => obj is Cast other && Equals(other);

        public bool Equals(Cast other)
            => Id == other.Id
            && Token == other.Token
            && IsOn == other.IsOn
            && Aim == other.Aim;

        public override int GetHashCode() => HashCode.Combine(Id, Token, IsOn, Aim);

        public override string ToString() => $"<{Id} {Token} {IsOn} {Aim}>";

        public static bool operator ==(Cast left, Cast right) => left.Equals(right);

        public static bool operator !=(Cast left, Cast right) => !(left == right);
    }
}
