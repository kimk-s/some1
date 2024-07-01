using Some1.Play.Info;

namespace Some1.Play.Front
{
    public interface IRegionSectionFront
    {
        RegionSectionId Id { get; }
        SectionType Type { get; }
        SectionKind Kind { get; }
        Area Area { get; }
    }
}
