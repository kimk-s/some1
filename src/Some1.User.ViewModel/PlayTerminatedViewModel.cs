using System;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class PipeTerminatedViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PipeTerminatedViewModel(ReactiveCommand<Unit> quit)
        {
            Quit = quit;
        }

        public ReactiveCommand<Unit> Quit { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
