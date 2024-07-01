using System;
using System.Linq;
using R3;
using Some1.Play.Front;
using Some1.UI;

namespace Some1.User.ViewModel
{
    public sealed class PlayerGameMenuViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayerGameMenuViewModel(
            IPlayFront front,
            SharedCanExecute sharedCanExecute)
        {
            Argses = new PlayerGameArgsGroupViewModel(front, sharedCanExecute).AddTo(_disposables);

            Results = new PlayerGameResultGroupViewModel(front.Player.GameResults).AddTo(_disposables);

            Rankings = new PlayRankingGroupViewModel(front.Rankings).AddTo(_disposables);

            Close = front.Player.GameManager.Status
                .CombineLatest(front.Player.Game.Game, (type, game) => type is not null || game is not null)
                .Where(x => x)
                .AsUnitObservable()
                .Merge(Argses.Close);
        }

        public Observable<Unit> Close { get; }

        public PlayerGameArgsGroupViewModel Argses { get; }

        public PlayerGameResultGroupViewModel Results { get; }

        public PlayRankingGroupViewModel Rankings { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
