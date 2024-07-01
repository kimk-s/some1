namespace Some1.Play.Core.Options
{
    public class PlayLoopOptions
    {
        public const string PlayLoop = "PlayLoop";
        public int FPS { get; set; } = 30;
        public int MinFPS { get; set; } = 30;
        public float TimeScale { get; set; } = 1;
        public float LogOnOverRate { get; set; } = 0.1f;
    }
}
