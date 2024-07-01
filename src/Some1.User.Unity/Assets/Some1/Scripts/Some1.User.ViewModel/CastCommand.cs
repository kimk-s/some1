using System;
using Some1.Play.Info;

namespace Some1.User.ViewModel
{
    public readonly struct CastCommand : IEquatable<CastCommand>
    {
        public CastCommand(CastId id, bool isOn, Aim aim)
        {
            Id = id;
            IsOn = isOn;
            Aim = aim;
        }

        public CastId Id { get; }

        public bool IsOn { get; }

        public Aim Aim { get; }

        public override bool Equals(object? obj) => obj is CastCommand other && Equals(other);

        public bool Equals(CastCommand other) => Id == other.Id && IsOn == other.IsOn && Aim.Equals(other.Aim);

        public override int GetHashCode() => HashCode.Combine(Id, IsOn, Aim);

        public override string ToString() => $"<{Id} {IsOn} {Aim}>";

        public static bool operator ==(CastCommand left, CastCommand right) => left.Equals(right);

        public static bool operator !=(CastCommand left, CastCommand right) => !(left == right);
    }
}
