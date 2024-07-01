using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class RegionSection : IRegionSection
    {
        internal RegionSection(RegionSectionInfo info, Area area)
        {
            Id = info.Id;
            Type = info.Type;
            Area = area;
        }

        public RegionSectionId Id { get; }

        public SectionType Type { get; }

        public Area Area { get; }
    }
}
