using System.Collections.Generic;

namespace Some1.Play.Info.Alpha
{
    public static class AlphaRegionSectionInfosFactory
    {
        public static IEnumerable<RegionSectionInfo> Create() => new RegionSectionInfo[]
        {
            new(new(RegionId.Region1, 0), SectionType.Field, SectionKind.None),
            new(new(RegionId.Region1, 1), SectionType.Field, SectionKind.None),
            new(new(RegionId.Region1, 2), SectionType.Field, SectionKind.None),
            new(new(RegionId.Region1, 3), SectionType.Field, SectionKind.None),
            new(new(RegionId.Region1, 4), SectionType.Field, SectionKind.None),
            new(new(RegionId.Region1, 5), SectionType.Field, SectionKind.None),
            new(new(RegionId.Region1, 6), SectionType.Field, SectionKind.None),
            new(new(RegionId.Region1, 7), SectionType.Field, SectionKind.None),
            new(new(RegionId.Region1, 8), SectionType.Field, SectionKind.None),
            new(new(RegionId.RegionAlpha, 0), SectionType.Yard, SectionKind.Secondary),
            new(new(RegionId.RegionAlpha, 1), SectionType.Yard, SectionKind.Secondary),
            new(new(RegionId.RegionAlpha, 2), SectionType.Yard, SectionKind.Secondary),
            new(new(RegionId.RegionAlpha, 3), SectionType.Yard, SectionKind.Secondary),
            new(new(RegionId.RegionAlpha, 4), SectionType.Yard, SectionKind.Secondary),
            new(new(RegionId.RegionAlpha, 5), SectionType.Yard, SectionKind.Secondary),
            new(new(RegionId.RegionAlpha, 6), SectionType.Town, SectionKind.Primary),
            new(new(RegionId.RegionAlpha, 7), SectionType.Town, SectionKind.Primary),
            new(new(RegionId.RegionAlpha, 8), SectionType.Town, SectionKind.Primary),
        };
    }
}
