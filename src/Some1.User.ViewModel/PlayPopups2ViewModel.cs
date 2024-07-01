using System;
using System.Linq;
using Some1.Play.Front;
using Some1.Play.Info;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class PlayPopups2ViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly IPlayFront _front;
        private readonly ReactiveCommand<Unit> _startPipe;
        private readonly ReactiveCommand<Unit> _quit;

        public PlayPopups2ViewModel(IPlayFront front, ReactiveCommand<Unit> startPipe, ReactiveCommand<Unit> quit)
        {
            PipeError = front.PipeState
                .Select(x => x.Exception)
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            PipeTerminated = front.PipeState
                .Select(x => x.Status == PipeStatus.Terminated)
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            _front = front;
            _startPipe = startPipe;
            _quit = quit;
        }

        public ReadOnlyReactiveProperty<Exception?> PipeError { get; }

        public ReadOnlyReactiveProperty<bool> PipeTerminated { get; }

        public PipeErrorViewModel GetPipeErrorViewModel()
        {
            return new(
                _front,
                _startPipe,
                _quit);
        }

        public PipeTerminatedViewModel GetPipeTerminatedViewModel()
        {
            return new(_quit);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
