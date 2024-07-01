using System;
using System.Collections.Generic;

namespace Some1.Play.Info.Alpha
{
    public static class AlphaSeasonInfoFactory
    {
        public static IEnumerable<SeasonInfo> Create() => new SeasonInfo[]
        {
            new(SeasonId.Season1, SeasonType.Current),
            new(SeasonId.Season2, SeasonType.ComingSoon),
        };
    }
}
