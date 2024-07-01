using System;
using System.Threading;
using R3;
using Some1.Play.Front;

namespace Some1.User.ViewModel
{
    public sealed class PlayerWelcomeViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly CancellationTokenSource _cts = new();

        public PlayerWelcomeViewModel(IPlayFront front, ReactiveProperty<bool> unarySharedCanExecute)
        {
            Active = front.Time.FrameCount
                .CombineLatest(
                    front.Player.Welcome.Welcome,
                    (frameCount, welcome) => frameCount > 0 && !welcome)
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            Set = new ReactiveCommand<Unit>(unarySharedCanExecute, true).AddTo(_disposables);
            Set.SubscribeAwait(
                unarySharedCanExecute,
                async (_, ct) => await front.SetWelcomeAsync(ct),
                AwaitOperation.Drop);
        }

        public ReadOnlyReactiveProperty<bool> Active { get; }

        public ReactiveCommand<Unit> Set { get; }

        public void Dispose()
        {
            _cts.Cancel();
            _disposables.Dispose();
        }
    }
}
