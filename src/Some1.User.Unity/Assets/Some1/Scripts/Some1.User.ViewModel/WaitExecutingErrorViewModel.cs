using System;
using R3;
using Some1.UI;

namespace Some1.User.ViewModel
{
    public sealed class WaitExecutingErrorViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public WaitExecutingErrorViewModel(
            ReactiveProperty<TaskState> executingState,
            SharedCanExecute sharedCanExecute)
        {
            Error = executingState.Select(x => x.Exception).ToReadOnlyReactiveProperty().AddTo(_disposables);

            ClearError = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);
            ClearError.Subscribe(_ => executingState.Value = TaskState.None);
        }

        public ReadOnlyReactiveProperty<Exception?> Error { get; }

        public ReactiveCommand<Unit> ClearError { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
