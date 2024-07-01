using System.Collections.Generic;
using Some1.Play.Info;

namespace Some1.Play.Front
{
    public interface IRegionFront
    {
        RegionId Id { get; }
        Area Area { get; }
        SeasonId? SeasonId { get; }
        IReadOnlyList<IRegionSectionFront> Sections { get; }
    }
}
