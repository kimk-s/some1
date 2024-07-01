using System.Collections.Generic;
using System.Numerics;
using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IRegionGroup
    {
        IReadOnlyDictionary<RegionId, IRegion> All { get; }

        IRegionSection Get(RegionSectionId id);
        IRegionSection? Get(Vector2 position);
        Vector2 GetTownWarpPosition(Vector2? fromPosition);
        Vector2 GetFieldWarpPosition(Vector2? fromPosition);
    }
}
