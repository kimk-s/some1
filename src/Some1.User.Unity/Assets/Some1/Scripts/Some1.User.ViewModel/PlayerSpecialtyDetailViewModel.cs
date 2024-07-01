using System;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;

namespace Some1.User.ViewModel
{
    public sealed class PlayerSpecialtyDetailViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayerSpecialtyDetailViewModel(IPlayFront front, SpecialtyId specialtyId)
        {
            var specialtyFront = front.Player.Specialties.All[specialtyId];
            Id = specialtyFront.Id;
            Star = specialtyFront.Star.Connect().AddTo(_disposables);
            NumberTimeAgo = specialtyFront.NumberTimeAgo.Connect().AddTo(_disposables);
            Season = specialtyFront.Season;
            Region = specialtyFront.Region;
        }

        public SpecialtyId Id { get; }

        public ReadOnlyReactiveProperty<Leveling> Star { get; }

        public ReadOnlyReactiveProperty<TimeSpan?> NumberTimeAgo { get; }

        public SeasonId Season { get; }

        public RegionId Region { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
