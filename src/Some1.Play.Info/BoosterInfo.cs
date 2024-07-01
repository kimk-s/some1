namespace Some1.Play.Info
{
    public sealed class BoosterInfo
    {
        public BoosterInfo(BoosterId id, float seconds)
        {
            Id = id;
            Seconds = seconds;
        }

        public BoosterId Id { get; }

        public float Seconds { get; }
    }
}
