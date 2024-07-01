namespace Some1.Wait.Front.Internal
{
    internal sealed class WaitWaitFront : IWaitWaitFront
    {
        public WaitWaitFront(bool maintenance)
        {
            Maintenance = maintenance;
        }

        public bool Maintenance { get; }
    }
}
