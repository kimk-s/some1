using Some1.Wait.Front;

namespace Some1.User.ViewModel
{
    public sealed class WaitWaitViewModel
    {
        public WaitWaitViewModel(IWaitWaitFront front)
        {
            Maintenance = front.Maintenance;
        }

        public bool Maintenance { get; }
    }
}
