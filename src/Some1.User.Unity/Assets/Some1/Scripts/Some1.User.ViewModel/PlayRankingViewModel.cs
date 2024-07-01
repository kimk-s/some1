using System;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;

namespace Some1.User.ViewModel
{
    public sealed class PlayRankingViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayRankingViewModel(IRankingFront front)
        {
            Active = front.Ranking.Select(x => x.Number > 0).ToReadOnlyReactiveProperty().AddTo(_disposables);

            Number = front.Ranking.Select(x => x.Number).ToReadOnlyReactiveProperty().AddTo(_disposables);

            Score = front.Ranking.Select(x => x.Score).ToReadOnlyReactiveProperty().AddTo(_disposables);

            Medal = front.Ranking.Select(x => x.Medal).ToReadOnlyReactiveProperty().AddTo(_disposables);

            Like = front.Ranking.Select(x => x.Title.Like).ToReadOnlyReactiveProperty().AddTo(_disposables);

            StarGrade = front.Ranking
                .Select(x => new Leveling(x.Title.Star, 1, PlayTitleViewModel.MaxStar, LevelingMethod.Plain))
                .ToReadOnlyReactiveProperty();

            PlayerId = front.Ranking
                .Select(x => x.Title.PlayerId)
                .ToReadOnlyReactiveProperty();

            IsMine = front.IsMine.Connect().AddTo(_disposables);
        }

        public ReadOnlyReactiveProperty<bool> Active { get; }

        public ReadOnlyReactiveProperty<byte> Number { get; }

        public ReadOnlyReactiveProperty<int> Score { get; }

        public ReadOnlyReactiveProperty<Medal> Medal { get; }

        public ReadOnlyReactiveProperty<byte> Like { get; }

        public ReadOnlyReactiveProperty<Leveling> StarGrade { get; }

        public ReadOnlyReactiveProperty<PlayerId> PlayerId { get; }

        public ReadOnlyReactiveProperty<bool> IsMine { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
