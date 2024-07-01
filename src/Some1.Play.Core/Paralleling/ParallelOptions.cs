using System;

namespace Some1.Play.Core.Paralleling
{
    public sealed class ParallelOptions
    {
        public int Count { get; set; } = Environment.ProcessorCount;
    }

    public static class ParallelOptionsExtensions
    {
        public static int GetSafeCount(this ParallelOptions x)
            => x.Count < 1 ? Environment.ProcessorCount : x.Count;
    }
}
