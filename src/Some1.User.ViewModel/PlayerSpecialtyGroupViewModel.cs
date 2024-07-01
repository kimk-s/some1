using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;

namespace Some1.User.ViewModel
{
    public sealed class PlayerSpecialtyGroupViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly Func<SpecialtyId, PlayerSpecialtyDetailViewModel> _createDetail;

        public PlayerSpecialtyGroupViewModel(IPlayFront front)
        {
            Specialties = front.Player.Specialties.All.ToDictionary(
                x => x.Key,
                x => new PlayerSpecialtyViewModel(x.Value)
                .AddTo(_disposables));

            Star = front.Player.Specialties.Star.AddTo(_disposables);

            _createDetail = (SpecialtyId id) => new PlayerSpecialtyDetailViewModel(front, id);
        }

        public IReadOnlyDictionary<SpecialtyId, PlayerSpecialtyViewModel> Specialties { get; }

        public ReadOnlyReactiveProperty<Leveling> Star { get; }

        public PlayerSpecialtyDetailViewModel CreateDetail(SpecialtyId id) => _createDetail(id);

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
