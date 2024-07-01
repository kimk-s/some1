using System;
using R3;
using Some1.Wait.Front;

namespace Some1.User.ViewModel
{
    public sealed class PlayUserViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposable = new();

        public PlayUserViewModel(IWaitFront front)
        {
            User = front.User.Select(x => x is null ? null : new WaitUserViewModel(x))
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposable);
        }

        public ReadOnlyReactiveProperty<WaitUserViewModel?> User { get; }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
