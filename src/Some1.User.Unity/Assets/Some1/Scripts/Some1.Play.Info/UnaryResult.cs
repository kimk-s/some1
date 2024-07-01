using System;
using MemoryPack;

namespace Some1.Play.Info
{
    [MemoryPackable]
    public readonly partial struct UnaryResult : IEquatable<UnaryResult>
    {
        public UnaryResult(Unary? unary, bool result)
        {
            Unary = unary;
            Result = result;
        }

        public Unary? Unary { get; }

        public bool Result { get; }

        public override bool Equals(object? obj) => obj is UnaryResult other && Equals(other);

        public bool Equals(UnaryResult other) => Unary.Equals(other.Unary) && Result == other.Result;

        public override int GetHashCode() => HashCode.Combine(Unary, Result);

        public override string ToString() => $"<{Unary} {Result}>";

        public static bool operator ==(UnaryResult left, UnaryResult right) => left.Equals(right);

        public static bool operator !=(UnaryResult left, UnaryResult right) => !(left == right);
    }
}
