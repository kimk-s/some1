using System;
using R3;
using Some1.Play.Front;

namespace Some1.User.ViewModel
{
    public sealed class PlayerGameToastViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayerGameToastViewModel(IPlayerGameToastFront front)
        {
            State = front.State.Connect().AddTo(_disposables);
        }

        public ReadOnlyReactiveProperty<PlayerGameToastState> State { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
