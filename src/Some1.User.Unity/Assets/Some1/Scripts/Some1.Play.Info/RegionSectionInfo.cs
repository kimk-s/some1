namespace Some1.Play.Info
{
    public sealed class RegionSectionInfo
    {
        public RegionSectionInfo(RegionSectionId id, SectionType type, SectionKind kind)
        {
            Id = id;
            Type = type;
            Kind = kind;
        }

        public RegionSectionId Id { get; }

        public SectionType Type { get; }

        public SectionKind Kind { get; }
    }
}
