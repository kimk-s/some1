namespace Some1.Play.Core.Options
{
    public class PlayerGroupOptions
    {
        public int Count { get; set; }
        public PlayBusyOptions Busy { get; set; } = null!;
        public float LoadThrottle { get; set; } = 0;
        public float SaveThrottle { get; set; } = 0.2f;
    }
}
