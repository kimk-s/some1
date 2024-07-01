using System;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;
using Some1.UI;

namespace Some1.User.ViewModel
{
    public sealed class PlayerGameArgsViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly Subject<Unit> _close = new();

        public PlayerGameArgsViewModel(
            GameMode id,
            IPlayFront front,
            SharedCanExecute sharedCanExecute)
        {
            _close.AddTo(_disposables);

            var f = front.Player.GameArgses.All[id];

            Id = f.Id;
            IsSelected = f.IsSelected;
            Score = f.Score;

            RankingActive = id == GameMode.Challenge;
            // Bug when number is changed but mine is not changed.
            Ranking = 
                (id == GameMode.Challenge
                    ? front.Rankings.Mine.Select(x => x?.Ranking.CurrentValue.Number ?? 0)
                    : Observable.Return((byte)0))
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            Select = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);
            Select.SubscribeAwait(
                sharedCanExecute,
                async (_, ct) =>
                {
                    await front.SelelctGameArgsAsync(Id, ct);
                    _close.OnNext(Unit.Default);
                },
                AwaitOperation.Drop);
        }

        internal Observable<Unit> Close => _close;

        public GameMode Id { get; }

        public ReadOnlyReactiveProperty<bool> IsSelected { get; }

        public ReadOnlyReactiveProperty<int> Score { get; }

        public bool RankingActive { get; }

        public ReadOnlyReactiveProperty<byte> Ranking { get; }

        public ReactiveCommand<Unit> Select { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
