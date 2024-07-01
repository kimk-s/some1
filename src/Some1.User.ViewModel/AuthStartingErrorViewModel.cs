using System;
using R3;
using Some1.UI;

namespace Some1.User.ViewModel
{
    public sealed class AuthStartingErrorViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public AuthStartingErrorViewModel(
            ReadOnlyReactiveProperty<TaskState> startingState,
            ReactiveCommand<Unit> start)
        {
            Error = startingState.Select(x => x.Exception).ToReadOnlyReactiveProperty().AddTo(_disposables);

            Start = start;
        }

        public ReadOnlyReactiveProperty<Exception?> Error { get; }

        public ReactiveCommand<Unit> Start { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
