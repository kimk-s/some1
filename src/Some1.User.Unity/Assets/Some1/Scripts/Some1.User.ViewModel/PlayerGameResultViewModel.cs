using System;
using System.Linq;
using Some1.Play.Front;
using Some1.Play.Info;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class PlayerGameResultViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayerGameResultViewModel(int id, IPlayerGameResultFront front)
        {
            Id = id;
            Active = front.Result.Select(x => x is not null).ToReadOnlyReactiveProperty().AddTo(_disposables);
            Success = front.Result.Select(x => x?.Success).ToReadOnlyReactiveProperty().AddTo(_disposables);
            Mode = front.Result.Select(x => x?.Game.Mode).ToReadOnlyReactiveProperty().AddTo(_disposables);
            Score = front.Result.Select(x => x?.Game.Score).ToReadOnlyReactiveProperty().AddTo(_disposables);
            Ago = front.EndTimeAgo;
            EndTime = front.Result.Select(x => x?.EndTime ?? DateTime.MinValue).ToReadOnlyReactiveProperty().AddTo(_disposables);
        }

        public int Id { get; }

        public ReadOnlyReactiveProperty<bool> Active { get; }

        public ReadOnlyReactiveProperty<bool?> Success { get; }

        public ReadOnlyReactiveProperty<GameMode?> Mode { get; } 

        public ReadOnlyReactiveProperty<int?> Score { get; }

        public ReadOnlyReactiveProperty<TimeSpan> Ago { get; }

        internal ReadOnlyReactiveProperty<DateTime> EndTime { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
