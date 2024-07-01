using System;
using R3;
using Some1.Auth.Front;
using Some1.UI;

namespace Some1.User.ViewModel
{
    public sealed class AuthViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly Func<SignInWithEmailViewModel> _createSignInWithEmailViewModel;
        private readonly Func<AuthStartingErrorViewModel> _createAuthStartingErrorViewModel;
        private readonly Func<AuthExecutingErrorViewModel> _createAuthExecutingErrorViewModel;

        public AuthViewModel(IAuthFront front, SharedCanExecute sharedCanExecute)
        {
            var startingState = new ReactiveProperty<TaskState>().AddTo(_disposables);
            var executingState = new ReactiveProperty<TaskState>().AddTo(_disposables);

            StartingError = startingState.Select(x => x.Exception).ToReadOnlyReactiveProperty();
            ExecutingError = executingState.Select(x => x.Exception).ToReadOnlyReactiveProperty();

            Destroy = front.User.Select(x => x is not null).ToReadOnlyReactiveProperty().AddTo(_disposables);

            PageActive = startingState
                .CombineLatest(Destroy, (x, y) => x.IsCompletedSuccessfully && !y)
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            IsExecutingActive = sharedCanExecute.Select(x => !x).ToReadOnlyReactiveProperty().AddTo(_disposables);

            Start = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);
            Start.SubscribeAwait(
                sharedCanExecute,
                async (_, ct) => await front.PassAsync(ct),
                AwaitOperation.Drop,
                startingState);

            SignInWithGoogle = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);
            SignInWithGoogle.SubscribeAwait(
                sharedCanExecute,
                async (_, ct) => await front.SignInWithGoogleAsync(ct),
                AwaitOperation.Drop,
                executingState);

            OpenSignInWithEmail = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);

            _createSignInWithEmailViewModel = () => new(front, executingState, sharedCanExecute);
            _createAuthStartingErrorViewModel = () => new(startingState, Start);
            _createAuthExecutingErrorViewModel = () => new(executingState, sharedCanExecute);
        }

        public ReadOnlyReactiveProperty<bool> Destroy { get; }

        public ReadOnlyReactiveProperty<bool> PageActive { get; }

        public ReadOnlyReactiveProperty<bool> IsExecutingActive { get; }

        public ReadOnlyReactiveProperty<Exception?> StartingError { get; }

        public ReadOnlyReactiveProperty<Exception?> ExecutingError { get; }

        public ReactiveCommand<Unit> Start { get; }

        public ReactiveCommand<Unit> SignInWithGoogle { get; }

        public ReactiveCommand<Unit> OpenSignInWithEmail { get; }

        public SignInWithEmailViewModel CreateSignInWithEmailViewModel() => _createSignInWithEmailViewModel();

        public AuthStartingErrorViewModel CreateAuthStartingErrorViewModel() => _createAuthStartingErrorViewModel();

        public AuthExecutingErrorViewModel CreateAuthExecutingErrorViewModel() => _createAuthExecutingErrorViewModel();

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
