using System;
using Some1.Play.Front;
using Some1.Play.Info;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class PlayerSeasonViewModel : IDisposable
    {
        public PlayerSeasonViewModel(SeasonId id, IPlayFront front)
        {
            var f = front.Player.Seasons[id];

            Id = f.Id;
            Type = f.Type;
            Star = f.Star;
        }

        public SeasonId Id { get; }

        public SeasonType Type { get; }

        public ReadOnlyReactiveProperty<Leveling?> Star { get; }

        public void Dispose()
        {
        }
    }
}
