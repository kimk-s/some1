using System;
using System.Linq;
using R3;
using Some1.UI;

namespace Some1.Prefs.UI
{
    public sealed class CultureGroupExecutingErrorViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public CultureGroupExecutingErrorViewModel(ReactiveProperty<TaskState> executingState)
        {
            Message = executingState.Select(x => x.Exception?.ToString()).ToReadOnlyReactiveProperty().AddTo(_disposables);

            OK = new ReactiveCommand<Unit>().AddTo(_disposables);
            OK.Subscribe(_ => executingState.Value = TaskState.None);
        }
        public ReadOnlyReactiveProperty<string?> Message { get; }

        public ReactiveCommand<Unit> OK { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
