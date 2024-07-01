using MemoryPack;

namespace Some1.Wait.Back
{
    [MemoryPackable]
    public sealed partial class WaitWaitBack
    {
        public WaitWaitBack(bool maintenance)
        {
            Maintenance = maintenance;
        }

        public bool Maintenance { get; }
    }
}
