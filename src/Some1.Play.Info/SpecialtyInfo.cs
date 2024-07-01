namespace Some1.Play.Info
{
    public sealed class SpecialtyInfo
    {
        public SpecialtyInfo(SpecialtyId id, SeasonId season, RegionId region)
        {
            Id = id;
            Season = season;
            Region = region;
        }

        public SpecialtyId Id { get; }

        public SeasonId Season { get; }

        public RegionId Region { get; }
    }
}
