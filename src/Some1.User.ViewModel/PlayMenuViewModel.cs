using System;
using R3;
using Some1.Play.Front;
using Some1.UI;
using Some1.Wait.Front;

namespace Some1.User.ViewModel
{
    public sealed class PlayMenuViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayMenuViewModel(
            IPlayFront playFront,
            IWaitFront waitFront,
            SharedCanExecute sharedCanExecute)
        {
            MessageList = new MessageListViewModel().AddTo(_disposables);

            Title = new PlayTitleViewModel(playFront).AddTo(_disposables);

            Characters = new PlayerCharacterGroupViewModel(playFront, MessageList, sharedCanExecute).AddTo(_disposables);

            Specialties = new PlayerSpecialtyGroupViewModel(playFront).AddTo(_disposables);

            Premium = new PlayerPremiumViewModel(playFront, waitFront, sharedCanExecute).AddTo(_disposables);

            Seasons = new PlayerSeasonGroupViewModel(playFront).AddTo(_disposables);

            User = new PlayUserViewModel(waitFront).AddTo(_disposables);

            Unary = new PlayUnaryViewModel(
                playFront,
                x => false,
                sharedCanExecute).AddTo(_disposables);

            BuyResult = new PlayBuyResultViewModel(waitFront).AddTo(_disposables);
        }

        public PlayTitleViewModel Title { get; }

        public PlayerCharacterGroupViewModel Characters { get; }

        public PlayerSpecialtyGroupViewModel Specialties { get; }

        public PlayerPremiumViewModel Premium { get; }

        public PlayerSeasonGroupViewModel Seasons { get; }

        public PlayUserViewModel User { get; }

        public PlayUnaryViewModel Unary { get; }

        public PlayBuyResultViewModel BuyResult { get; }

        public MessageListViewModel MessageList { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
