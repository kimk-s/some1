using System;
using System.Drawing;
using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IFloorFront
    {
        ReadOnlyReactiveProperty<FloorState?> State { get; }
    }

    public readonly struct FloorState : IEquatable<FloorState>
    {
        public FloorState(Point id, Area area, FloorType type)
        {
            Id = id;
            Area = area;
            Type = type;
        }

        public Point Id { get; }

        public Area Area { get; }

        public FloorType Type { get; }

        public override bool Equals(object? obj) => obj is FloorState other && Equals(other);

        public bool Equals(FloorState other) => Id.Equals(other.Id) && Area.Equals(other.Area) && Type.Equals(other.Type);

        public override int GetHashCode() => HashCode.Combine(Id, Area, Type);

        public override string ToString() => $"<{Id} {Area} {Type}>";

        public static bool operator ==(FloorState left, FloorState right) => left.Equals(right);

        public static bool operator !=(FloorState left, FloorState right) => !(left == right);
    }

    public readonly struct FloorType : IEquatable<FloorType>
    {
        public const int VariationCount = 5;

        public FloorType(RegionId regionId, SectionKind sectionKind, FloorRole role, int variation)
        {
            RegionId = regionId;
            SectionKind = sectionKind;
            Role = role;
            Variation = variation;
        }

        public RegionId RegionId { get; }

        public SectionKind SectionKind { get; }

        public FloorRole Role { get; }

        public int Variation { get; }

        public override bool Equals(object? obj) => obj is FloorType other && Equals(other);

        public bool Equals(FloorType other) => RegionId == other.RegionId && SectionKind == other.SectionKind && Role == other.Role && Variation == other.Variation;

        public override int GetHashCode() => HashCode.Combine(RegionId, SectionKind, Role, Variation);

        public override string ToString() => $"<{RegionId} {SectionKind} {Role} {Variation}>";

        public static bool operator ==(FloorType left, FloorType right) => left.Equals(right);

        public static bool operator !=(FloorType left, FloorType right) => !(left == right);
    }

    public enum FloorRole
    {
        Plain,
        Road,
        Plaza,
    }
}
