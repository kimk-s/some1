namespace Some1.Play.Info
{
    public sealed class RegionInfo
    {
        public RegionInfo(RegionId id, SeasonId? seasonId)
        {
            Id = id;
            SeasonId = seasonId;
        }

        public RegionId Id { get; }

        public SeasonId? SeasonId { get; }
    }
}
