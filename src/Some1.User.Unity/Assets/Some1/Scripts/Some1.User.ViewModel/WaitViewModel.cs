using System;
using System.Linq;
using R3;
using Some1.Auth.Front;
using Some1.UI;
using Some1.Wait.Front;

namespace Some1.User.ViewModel
{
    public sealed class WaitViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly ReactiveProperty<WaitStopResult> _stopResult = new();
        private readonly Func<WaitUserDetailViewModel> _createWaitUserDetailViewModel;
        private readonly Func<WaitPlayServerGroupViewModel> _createWaitPlayServerGroupViewModel;
        private readonly Func<WaitPlayServerDetailViewModel> _createWaitPlayServerDetailViewModel;
        private readonly Func<WaitStartingErrorViewModel> _createWaitStartingErrorViewModel;
        private readonly Func<WaitExecutingErrorViewModel> _createWaitExecutingErrorViewModel;

        public WaitViewModel(IWaitFront waitFront, IAuthFront authFront, SharedCanExecute sharedCanExecute)
        {
            SharedCanExecute = sharedCanExecute;

            var startingState = new ReactiveProperty<TaskState>().AddTo(_disposables);
            var executingState = new ReactiveProperty<TaskState>().AddTo(_disposables);

            StartingState = startingState;
            ExecutingState = executingState;

            _stopResult.AddTo(_disposables);

            authFront.User.Where(x => x == null)
                .Subscribe(_ => Stop(WaitStopResult.SignedOut))
                .AddTo(_disposables);

            waitFront.Reset();

            User = waitFront.User
                .Select(x => x is null ? null : new WaitUserViewModel(x))
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            Wait = waitFront.Wait
                .Select(x => x is null ? null : new WaitWaitViewModel(x))
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            OpenPlayServerGroup = new ReactiveCommand<Unit>().AddTo(_disposables);

            waitFront.SelectedPlay
                .Where(x => x is not null)
                .Subscribe(_ => Stop(WaitStopResult.Play))
                .AddTo(_disposables);

            SelectedPlayServer = waitFront.SelectedPlayServer
                .Select(x => x is null ? null : new WaitPlayServerSelectedViewModel(x, OpenPlayServerGroup))
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            Start = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);
            Start.SubscribeAwait(
                sharedCanExecute,
                async (_, ct) => await waitFront.StartAsync(ct),
                AwaitOperation.Drop,
                startingState);

            SelectAutomaticPlayChannel = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);
            SelectAutomaticPlayChannel.SubscribeAwait(
                sharedCanExecute,
                async (x, ct) => await waitFront.SelectPlayChannelAsync(IWaitFront.AutomaticPlayChannelNumber, ct),
                AwaitOperation.Drop,
                executingState);

            OpenUser = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);

            OpenPlayServerDetail = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);

            _createWaitUserDetailViewModel = () => new(User, authFront, executingState, sharedCanExecute);

            _createWaitPlayServerGroupViewModel = () => new(waitFront, executingState, sharedCanExecute);

            _createWaitPlayServerDetailViewModel = () => new(
                waitFront,
                waitFront.SelectedPlayServer.CurrentValue?.Id ?? throw new InvalidOperationException(),
                executingState,
                sharedCanExecute);

            _createWaitStartingErrorViewModel = () => new(startingState, Start);

            _createWaitExecutingErrorViewModel = () => new(executingState, sharedCanExecute);
        }

        public ReactiveProperty<bool> SharedCanExecute { get; }

        public ReadOnlyReactiveProperty<WaitStopResult> StopResult => _stopResult;

        public ReadOnlyReactiveProperty<WaitUserViewModel?> User { get; }

        public ReadOnlyReactiveProperty<WaitWaitViewModel?> Wait { get; }

        public ReadOnlyReactiveProperty<WaitPlayServerSelectedViewModel?> SelectedPlayServer { get; }

        public ReadOnlyReactiveProperty<TaskState> StartingState { get; }

        public ReadOnlyReactiveProperty<TaskState> ExecutingState { get; }

        public ReactiveCommand<Unit> Start { get; }

        public ReactiveCommand<Unit> OpenUser { get; }

        public ReactiveCommand<Unit> OpenPlayServerGroup { get; }

        public ReactiveCommand<Unit> SelectAutomaticPlayChannel { get; }

        public ReactiveCommand<Unit> OpenPlayServerDetail { get; }

        public WaitUserDetailViewModel CreateWaitUserDetailViewModel() => _createWaitUserDetailViewModel();

        public WaitPlayServerGroupViewModel CreateWaitPlayServerGroupViewModel() => _createWaitPlayServerGroupViewModel();

        public WaitPlayServerDetailViewModel CreateWaitPlayServerDetailViewModel() => _createWaitPlayServerDetailViewModel();

        public WaitStartingErrorViewModel CreateWaitStartingErrorViewModel() => _createWaitStartingErrorViewModel();

        public WaitExecutingErrorViewModel CreateWaitExecutingErrorViewModel() => _createWaitExecutingErrorViewModel();

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void Stop(WaitStopResult stopResult)
        {
            if (stopResult == WaitStopResult.None)
            {
                throw new ArgumentOutOfRangeException(nameof(stopResult));
            }

            if (StopResult.CurrentValue != WaitStopResult.None)
            {
                return;
            }

            _stopResult.Value = stopResult;
        }
    }
}
