using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class NonPlayerGenerationInfoGroup
    {
        public NonPlayerGenerationInfoGroup(IEnumerable<NonPlayerGenerationInfo> all)
        {
            ByRegionSectionId = all.ToDictionary(x => x.RegionSectionId);
        }

        public IReadOnlyDictionary<RegionSectionId, NonPlayerGenerationInfo> ByRegionSectionId { get; }
    }
}
