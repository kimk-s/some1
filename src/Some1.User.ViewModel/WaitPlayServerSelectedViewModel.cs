using R3;
using Some1.Wait.Front;

namespace Some1.User.ViewModel
{
    public sealed class WaitPlayServerSelectedViewModel
    {
        public WaitPlayServerSelectedViewModel(IWaitPlayServerFront front, ReactiveCommand<Unit> openPlayServerGroup)
        {
            Id = front.Id;
            OpeningSoon = front.OpeningSoon;
            Maintenance = front.Maintenance;
            IsFull = front.IsFull;
            OpenPlayServerGroup = openPlayServerGroup;
        }

        public WaitPlayServerFrontId Id { get; }

        public bool OpeningSoon { get; }

        public bool Maintenance { get; }

        public bool IsFull { get; }

        public ReactiveCommand<Unit> OpenPlayServerGroup { get; }
    }
}
