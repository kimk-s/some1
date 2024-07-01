using System;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal readonly struct Tile : IEquatable<Tile>
    {
        internal Tile(BumpLevel? bumpLevel) => BumpLevel = bumpLevel;

        internal BumpLevel? BumpLevel { get; }

        public override bool Equals(object? obj) => obj is Tile other && Equals(other);

        public bool Equals(Tile other) => BumpLevel == other.BumpLevel;

        public override int GetHashCode() => HashCode.Combine(BumpLevel);

        public override string ToString() => BumpLevel?.ToString() ?? "-";

        public static bool operator ==(Tile left, Tile right) => left.Equals(right);

        public static bool operator !=(Tile left, Tile right) => !(left == right);
    }
}
