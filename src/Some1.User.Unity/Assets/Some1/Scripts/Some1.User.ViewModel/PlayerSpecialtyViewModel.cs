using System;
using Some1.Play.Front;
using Some1.Play.Info;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class PlayerSpecialtyViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayerSpecialtyViewModel(IPlayerSpecialtyFront front)
        {
            Id = front.Id;
            Star = front.Star.Connect().AddTo(_disposables);
            NumberTimeAgo = front.NumberTimeAgo.Connect().AddTo(_disposables);
        }

        public SpecialtyId Id { get; }

        public ReadOnlyReactiveProperty<Leveling> Star { get; }

        public ReadOnlyReactiveProperty<TimeSpan?> NumberTimeAgo { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
