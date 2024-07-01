using System;

namespace Some1.Wait.Front
{
    public readonly struct WaitPlayServerFrontId : IEquatable<WaitPlayServerFrontId>
    {
        public WaitPlayServerFrontId(string region, string city)
        {
            Region = region;
            City = city;
        }

        public string Region { get; }
        public string City { get; }

        public override bool Equals(object? obj) => obj is WaitPlayServerFrontId other && Equals(other);

        public bool Equals(WaitPlayServerFrontId other) => Region == other.Region && City == other.City;

        public override int GetHashCode() => HashCode.Combine(Region, City);

        public static bool operator ==(WaitPlayServerFrontId left, WaitPlayServerFrontId right) => left.Equals(right);

        public static bool operator !=(WaitPlayServerFrontId left, WaitPlayServerFrontId right) => !(left == right);
    }
}
