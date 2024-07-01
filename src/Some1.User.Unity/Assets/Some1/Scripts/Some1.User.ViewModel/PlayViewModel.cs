using System;
using System.Linq;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;
using Some1.UI;
using Some1.Wait.Front;

namespace Some1.User.ViewModel
{
    public sealed class PlayViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly ReactiveProperty<PlayStopResult> _stopResult = new();

        public PlayViewModel(IPlayFront playFront, IWaitFront waitFront, SharedCanExecute sharedCanExecute)
        {
            _stopResult.AddTo(_disposables);

            playFront.PipeState
                .Where(x => x.Status == PipeStatus.Finished)
                .Subscribe(_ => Stop(PlayStopResult.Finished))
                .AddTo(_disposables);

            TitleActive = playFront.Time.FrameCount.Select(x => x == 0).ToReadOnlyReactiveProperty().AddTo(_disposables);
            LetterBoxActive = playFront.Time.FrameCount.Select(x => x != 0).ToReadOnlyReactiveProperty().AddTo(_disposables);

            Page = new PlayPageViewModel(playFront, sharedCanExecute).AddTo(_disposables);

            Popups = new PlayPopupsViewModel(playFront, waitFront, sharedCanExecute).AddTo(_disposables);

            var startPipeSharedCanExecute = new ReactiveProperty<bool>(true).AddTo(_disposables);
            var connecting = startPipeSharedCanExecute.Select(x => !x).ToReadOnlyReactiveProperty();
            Page2 = new PlayPage2ViewModel(playFront, sharedCanExecute, connecting).AddTo(_disposables);

            Indicators = new PlayIndicatorsViewModel(playFront, Page.Joysticks.CastUiState, Page.Joysticks.WalkUiState, Popups.Emojis.LikeUiState).AddTo(_disposables);

            Camera = new PlayCameraViewModel(playFront.Player).AddTo(_disposables);

            Objects = new ObjectGroupViewModel(playFront.Objects, playFront.Player.Object).AddTo(_disposables);

            Floors = new FloorGroupViewModel(playFront.Floors).AddTo(_disposables);

            LocalTimeScale = playFront.LocalTimeScale;

            StartPipe = new ReactiveCommand<Unit>(startPipeSharedCanExecute, true).AddTo(_disposables);
            StartPipe.SubscribeAwait(
                startPipeSharedCanExecute,
                async (_, ct) => await playFront.StartPipeAsync(ct),
                AwaitOperation.Drop);

            UpdatePipe = new ReactiveCommand<float>().AddTo(_disposables);
            UpdatePipe.Subscribe(x => playFront.UpdatePipe(x));

            Quit = new ReactiveCommand<Unit>().AddTo(_disposables);
            Quit.Subscribe(_ => Stop(PlayStopResult.Quit));

            Popups2 = new PlayPopups2ViewModel(playFront, StartPipe, Quit).AddTo(_disposables);
        }

        public ReadOnlyReactiveProperty<PlayStopResult> StopResult => _stopResult;

        public ReadOnlyReactiveProperty<bool> TitleActive { get; }

        public ReadOnlyReactiveProperty<bool> LetterBoxActive { get; }

        public PlayPageViewModel Page { get; }

        public PlayPopupsViewModel Popups { get; }

        public PlayPage2ViewModel Page2 { get; }

        public PlayPopups2ViewModel Popups2 { get; }

        public PlayIndicatorsViewModel Indicators { get; }

        public PlayCameraViewModel Camera { get; }

        public ObjectGroupViewModel Objects { get; }

        public FloorGroupViewModel Floors { get; }

        public ReadOnlyReactiveProperty<float> LocalTimeScale { get; }

        public ReactiveCommand<Unit> StartPipe { get; }

        public ReactiveCommand<float> UpdatePipe { get; }

        public ReactiveCommand<Unit> Quit { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void Stop(PlayStopResult stopResult)
        {
            if (stopResult == PlayStopResult.None)
            {
                throw new ArgumentOutOfRangeException(nameof(stopResult));
            }

            if (_stopResult.Value != PlayStopResult.None)
            {
                return;
            }

            _stopResult.Value = stopResult;
        }
    }
}
