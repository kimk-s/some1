using System;

namespace Some1.Play.Info
{
    public readonly struct RegionSectionId : IEquatable<RegionSectionId>
    {
        public RegionSectionId(RegionId region, int section)
        {
            Region = region;
            Section = section;
        }

        public RegionId Region { get; }

        public int Section { get; }

        public override bool Equals(object? obj) => obj is RegionSectionId other && Equals(other);

        public bool Equals(RegionSectionId other) => Region == other.Region && Section == other.Section;

        public override int GetHashCode() => HashCode.Combine(Region, Section);

        public override string ToString() => $"<{Region} {Section}>";

        public static bool operator ==(RegionSectionId left, RegionSectionId right) => left.Equals(right);

        public static bool operator !=(RegionSectionId left, RegionSectionId right) => !(left == right);
    }
}
