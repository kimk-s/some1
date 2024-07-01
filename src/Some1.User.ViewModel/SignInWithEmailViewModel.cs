using System;
using R3;
using Some1.Auth.Front;
using Some1.UI;

namespace Some1.User.ViewModel
{
    public sealed class SignInWithEmailViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public SignInWithEmailViewModel(
            IAuthFront front,
            ReactiveProperty<TaskState> executingState,
            SharedCanExecute sharedCanExecute)
        {
            Email = new ReactiveProperty<string>().AddTo(_disposables);
            Password = new ReactiveProperty<string>().AddTo(_disposables);

            SignInWithEmail = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);
            SignInWithEmail.SubscribeAwait(
                sharedCanExecute,
                async (_, ct) => await front.SignInWithEmailAndPasswordAsync(Email.Value!, Password.Value!, ct),
                AwaitOperation.Drop,
                executingState);

            Back = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);
        }

        public ReactiveProperty<string> Email { get; }

        public ReactiveProperty<string> Password { get; }

        public ReactiveCommand<Unit> SignInWithEmail { get; }

        public ReactiveCommand<Unit> Back { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
