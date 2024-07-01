using Some1.Play.Info;

namespace Some1.Play.Front
{
    internal sealed class RegionSectionFront : IRegionSectionFront
    {
        internal RegionSectionFront(RegionSectionInfo info, Area area)
        {
            Id = info.Id;
            Type = info.Type;
            Kind = info.Kind;
            Area = area;
        }

        public RegionSectionId Id { get; }

        public SectionType Type { get; }

        public Area Area { get; }

        public SectionKind Kind { get; }
    }
}
