using System;
using MemoryPack;

namespace Some1.Play.Info
{
    [MemoryPackable]
    public readonly partial struct Unary : IEquatable<Unary>
    {
        public Unary(int token, UnaryType type, int param1, int param2, int param3)
        {
            Token = token;
            Type = type;
            Param1 = param1;
            Param2 = param2;
            Param3 = param3;
        }

        public int Token { get; }

        public UnaryType Type { get; }

        public int Param1 { get; }

        public int Param2 { get; }

        public int Param3 { get; }

        public override bool Equals(object? obj) => obj is Unary other && Equals(other);

        public bool Equals(Unary other) => Token == other.Token && Type == other.Type && Param1 == other.Param1 && Param2 == other.Param2 && Param3 == other.Param3;

        public override int GetHashCode() => HashCode.Combine(Token, Type, Param1, Param2, Param3);

        public override string ToString() => $"<{Token} {Type} {Param1} {Param2} {Param3}>";

        public static bool operator ==(Unary left, Unary right) => left.Equals(right);

        public static bool operator !=(Unary left, Unary right) => !(left == right);
    }
}
