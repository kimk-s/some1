namespace Some1.Play.Info
{
    public sealed class SeasonInfo
    {
        public SeasonInfo(SeasonId id, SeasonType type)
        {
            Id = id;
            Type = type;
        }

        public SeasonId Id { get; }

        public SeasonType Type { get; }
    }
}
