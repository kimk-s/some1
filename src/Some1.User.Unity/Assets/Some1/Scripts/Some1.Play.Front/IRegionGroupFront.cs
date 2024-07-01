using System.Collections.Generic;
using System.Numerics;
using Some1.Play.Info;

namespace Some1.Play.Front
{
    public interface IRegionGroupFront
    {
        IReadOnlyDictionary<RegionId, IRegionFront> All { get; }
        IReadOnlyDictionary<SeasonId, IRegionFront> BySeasonId { get; }

        IRegionSectionFront? Get(Vector2 position);
    }
}
