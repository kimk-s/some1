using System;
using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class RegionInfoGroup
    {
        public RegionInfoGroup(IEnumerable<RegionInfo> all)
        {
            ById = all.ToDictionary(x => x.Id, x => x);

            BySeasonId = all
                .Where(x => x.SeasonId is not null)
                .ToDictionary(x => x.SeasonId!.Value);
        }

        public IReadOnlyDictionary<RegionId, RegionInfo> ById { get; }

        public IReadOnlyDictionary<SeasonId, RegionInfo> BySeasonId { get; }
    }
}
