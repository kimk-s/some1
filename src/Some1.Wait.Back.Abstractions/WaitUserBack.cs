using MemoryPack;

namespace Some1.Wait.Back
{
    [MemoryPackable]
    public sealed partial class WaitUserBack
    {
        public WaitUserBack(string id, string playId, bool isPlaying, bool manager)
        {
            Id = id;
            PlayId = playId;
            IsPlaying = isPlaying;
            Manager = manager;
        }

        public string Id { get; }
        public string PlayId { get; }
        public bool IsPlaying { get; }
        public bool Manager { get; }
    }
}
