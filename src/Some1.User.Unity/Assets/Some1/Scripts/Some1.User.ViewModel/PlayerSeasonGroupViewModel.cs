using System;
using System.Collections.Generic;
using System.Linq;
using Some1.Play.Front;
using Some1.Play.Info;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class PlayerSeasonGroupViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayerSeasonGroupViewModel(IPlayFront front)
        {
            Seasons = front.Player.Seasons.Values
                .Select(x => new PlayerSeasonViewModel(x.Id, front).AddTo(_disposables))
                .ToDictionary(x => x.Id, x => x);
        }

        public IReadOnlyDictionary<SeasonId, PlayerSeasonViewModel> Seasons { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
