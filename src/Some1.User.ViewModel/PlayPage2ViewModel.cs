using System;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;

namespace Some1.User.ViewModel
{
    public sealed class PlayPage2ViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayPage2ViewModel(
            IPlayFront front,
            ReactiveProperty<bool> unarySharedCanExecute,
            ReadOnlyReactiveProperty<bool> connecting)
        {
            Welcome = new PlayerWelcomeViewModel(front, unarySharedCanExecute).AddTo(_disposables);

            ActiveConnectingBlocker = connecting;

            ActiveFinishingBlocker = front.PipeState
                .Select(x => x.Status == PipeStatus.Finishing)
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);
        }

        public PlayerWelcomeViewModel Welcome { get; }

        public ReadOnlyReactiveProperty<bool> ActiveConnectingBlocker { get; }

        public ReadOnlyReactiveProperty<bool> ActiveFinishingBlocker { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
