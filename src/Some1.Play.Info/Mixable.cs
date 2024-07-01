using System;
using System.Collections.Generic;

namespace Some1.Play.Info
{
    public readonly struct Mixable<T> : IEquatable<Mixable<T>> where T : struct
    {
        public Mixable(bool isMix, T value)
        {
            IsMix = isMix;
            Value = value;
        }

        public bool IsMix { get; }

        public T Value { get; }

        public override bool Equals(object? obj) => obj is Mixable<T> other && Equals(other);

        public bool Equals(Mixable<T> other) => IsMix == other.IsMix && EqualityComparer<T>.Default.Equals(Value, other.Value);

        public override int GetHashCode() => HashCode.Combine(IsMix, Value);

        public override string ToString() => $"<{IsMix} {Value}>";

        public static bool operator ==(Mixable<T> left, Mixable<T> right) => left.Equals(right);

        public static bool operator !=(Mixable<T> left, Mixable<T> right) => !(left == right);

        public static implicit operator Mixable<T>(T value) => new(false, value);
    }
}
