using System;
using R3;
using Some1.Play.Front;
using Some1.UI;
using Some1.Wait.Front;

namespace Some1.User.ViewModel
{
    public sealed class PlayPopupsViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayPopupsViewModel(
            IPlayFront playFront,
            IWaitFront waitFront,
            SharedCanExecute sharedCanExecute)
        {
            Menu = new PlayMenuViewModel(playFront, waitFront, sharedCanExecute).AddTo(_disposables);

            GameDetail = new PlayerGameDetailViewModel(playFront, sharedCanExecute).AddTo(_disposables);

            LocationDetail = new PlayLocationDetailViewModel(playFront, waitFront.SelectedPlay, sharedCanExecute).AddTo(_disposables);

            Emojis = new PlayerEmojiGroupViewModel(playFront, sharedCanExecute).AddTo(_disposables);

            GameMenu = new PlayerGameMenuViewModel(playFront, sharedCanExecute).AddTo(_disposables);
        }

        public PlayMenuViewModel Menu { get; }

        public PlayerGameDetailViewModel GameDetail { get; }

        public PlayLocationDetailViewModel LocationDetail { get; }

        public PlayerEmojiGroupViewModel Emojis { get; }

        public PlayerGameMenuViewModel GameMenu { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
