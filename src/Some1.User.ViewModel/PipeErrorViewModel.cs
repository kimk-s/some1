using System;
using Some1.Play.Front;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class PipeErrorViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PipeErrorViewModel(
            IPlayFront front,
            ReactiveCommand<Unit> restart,
            ReactiveCommand<Unit> quit)
        {
            Error = front.PipeState.Select(x => x.Exception).ToReadOnlyReactiveProperty().AddTo(_disposables);
            Restart = restart;
            Quit = quit;
        }

        public ReadOnlyReactiveProperty<Exception?> Error { get; }

        public ReactiveCommand<Unit> Restart { get; }

        public ReactiveCommand<Unit> Quit { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
