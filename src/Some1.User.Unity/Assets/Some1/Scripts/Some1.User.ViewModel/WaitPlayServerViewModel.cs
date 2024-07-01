using System;
using R3;
using Some1.Wait.Front;

namespace Some1.User.ViewModel
{
    public sealed class WaitPlayServerViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public WaitPlayServerViewModel(IWaitPlayServerFront front, ReactiveCommand<WaitPlayServerFrontId> select)
        {
            Id = front.Id;
            OpeningSoon = front.OpeningSoon;
            Maintenance = front.Maintenance;
            IsFull = front.IsFull;
            IsSelected = front.IsSelected;

            Select = new ReactiveCommand<Unit>().AddTo(_disposables);
            Select.Subscribe(_ => select.Execute(Id));
        }

        public WaitPlayServerFrontId Id { get; }

        public bool OpeningSoon { get; }

        public bool Maintenance { get; }

        public bool IsFull { get; }

        public bool IsSelected { get; }

        public ReactiveCommand<Unit> Select { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
