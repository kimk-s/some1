using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class NonPlayerSpawningInfoGroup
    {
        public NonPlayerSpawningInfoGroup(IEnumerable<NonPlayerSpawningInfo> all)
        {
            ByRegionSectionId = all.GroupBy(x => x.RegionSectionId)
                .ToDictionary(
                    x => x.Key,
                    x => (IReadOnlyList<NonPlayerSpawningInfo>)x.ToList());
        }

        public IReadOnlyDictionary<RegionSectionId, IReadOnlyList<NonPlayerSpawningInfo>> ByRegionSectionId { get; }
    }
}
