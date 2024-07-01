using System.Collections.Concurrent;

namespace Some1.Net
{
    public static class DuplexPipePairPool
    {
        private static readonly ConcurrentQueue<DuplexPipePair> s_queue = new();

        public static DuplexPipePair Rent()
        {
            if (!s_queue.TryDequeue(out var pair))
            {
                pair = new();
            }

            return pair;
        }

        internal static void Return(DuplexPipePair pair)
        {
            s_queue.Enqueue(pair);
        }
    }
}
