using System;
using Some1.Play.Front;
using Some1.Play.Info;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class PlayerGameResultDetailViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayerGameResultDetailViewModel(IPlayerGameResultFront front)
        {
            Result = front.Result.Connect().AddTo(_disposables);
            EndTimeAgo = front.EndTimeAgo.Connect().AddTo(_disposables);
        }

        public ReadOnlyReactiveProperty<GameResult?> Result { get; }

        public ReadOnlyReactiveProperty<TimeSpan> EndTimeAgo { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
