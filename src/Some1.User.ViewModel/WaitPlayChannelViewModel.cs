using System;
using R3;
using Some1.Wait.Front;

namespace Some1.User.ViewModel
{
    public sealed class WaitPlayChannelViewModel : IDisposable
    {
        public WaitPlayChannelViewModel(IWaitPlayChannelFront front, ReactiveCommand<int> select)
        {
            Number = front.Number;
            OpeningSoon = front.OpeningSoon;
            Maintenance = front.Maintenance;
            Busy = front.Busy;
            IsFull = front.IsFull;
            Select.Subscribe(_ => select.Execute(Number));
        }

        public int Number { get; }

        public bool OpeningSoon { get; }

        public bool Maintenance { get; }

        public float Busy { get; }

        public bool IsFull { get; }

        public ReactiveCommand<Unit> Select { get; } = new();

        public void Dispose()
        {
            ((IDisposable)Select).Dispose();
        }
    }
}
