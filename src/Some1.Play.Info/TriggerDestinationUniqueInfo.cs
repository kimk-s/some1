namespace Some1.Play.Info
{
    public sealed class TriggerDestinationUniqueInfo
    {
        public TriggerDestinationUniqueInfo(TriggerDestinationUniqueId id)
        {
            Id = id;
        }

        public TriggerDestinationUniqueId Id { get; }
    }

    public enum TriggerDestinationUniqueId
    {
        Transient,
        Scoped1,
        Scoped2,
    }
}
