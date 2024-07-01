using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IRegionSection
    {
        RegionSectionId Id { get; }
        SectionType Type { get; }
        Area Area { get; }
    }
}
