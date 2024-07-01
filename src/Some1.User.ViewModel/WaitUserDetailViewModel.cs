using System;
using R3;
using Some1.Auth.Front;
using Some1.UI;

namespace Some1.User.ViewModel
{
    public sealed class WaitUserDetailViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public WaitUserDetailViewModel(
            ReadOnlyReactiveProperty<WaitUserViewModel?> user,
            IAuthFront authFront,
            ReactiveProperty<TaskState> executingState,
            SharedCanExecute sharedCanExecute)
        {
            User = user;

            SignOut = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);
            SignOut.SubscribeAwait(
                sharedCanExecute,
                async (_, ct) => await authFront.SignOutAsync(ct),
                AwaitOperation.Drop,
                executingState);

            Back = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);
        }

        public ReadOnlyReactiveProperty<WaitUserViewModel?> User { get; }

        public ReactiveCommand<Unit> SignOut { get; }

        public ReactiveCommand<Unit> Back { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
