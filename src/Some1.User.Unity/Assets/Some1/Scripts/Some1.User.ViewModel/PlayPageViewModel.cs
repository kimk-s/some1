using System;
using System.Linq;
using R3;
using Some1.Play.Front;
using Some1.UI;

namespace Some1.User.ViewModel
{
    public sealed class PlayPageViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayPageViewModel(
            IPlayFront playFront,
            SharedCanExecute sharedCanExecute)
        {
            Active = playFront.Time.FrameCount.Select(x => x != 0).ToReadOnlyReactiveProperty().AddTo(_disposables);

            Face = new PlayerFaceViewModel(playFront).AddTo(_disposables);
            GameToast = new PlayerGameToastViewModel(playFront.Player.GameToast).AddTo(_disposables);
            Game = new PlayerGameViewModel(playFront.Player.Game).AddTo(_disposables);
            Emoji = new PlayPageEmojiViewModel(playFront.Player.Emojis).AddTo(_disposables);
            Location = new PlayLocationViewModel(playFront.Player.Object).AddTo(_disposables);
            GameReady = new PlayerGameReadyViewModel(playFront, sharedCanExecute).AddTo(_disposables);
            Joysticks = new PlayJoysticksViewModel(playFront).AddTo(_disposables);
        }

        public ReadOnlyReactiveProperty<bool> Active { get; }

        public PlayerFaceViewModel Face { get; }

        public PlayerGameToastViewModel GameToast { get; }

        public PlayerGameViewModel Game { get; }

        public PlayPageEmojiViewModel Emoji { get; }

        public PlayLocationViewModel Location { get; }

        public PlayerGameReadyViewModel GameReady { get; }

        public PlayJoysticksViewModel Joysticks { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
