using System.Collections.Generic;
using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IRegion
    {
        RegionId Id { get; }
        SeasonId? SeasonId { get; }
        Area Area { get; }
        IReadOnlyList<IRegionSection> Sections { get; }
    }
}
