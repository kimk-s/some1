using Some1.Play.Core.Paralleling;

namespace Some1.Play.Core.Options
{
    public class PlayOptions
    {
        public const string Play = "Play";

        public string Id { get; set; } = null!;
        public ParallelOptions Parallel { get; set; } = null!;
        public PlayerGroupOptions Players { get; set; } = null!;
    }
}
