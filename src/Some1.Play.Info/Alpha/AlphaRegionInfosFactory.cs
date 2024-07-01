using System.Collections.Generic;

namespace Some1.Play.Info.Alpha
{
    public static class AlphaRegionInfosFactory
    {
        public static IEnumerable<RegionInfo> Create() => new RegionInfo[]
        {
            new(RegionId.Region1, SeasonId.Season1),
            new(RegionId.RegionAlpha, null),
        };
    }
}
