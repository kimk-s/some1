using System;
using Some1.Play.Front;
using Some1.Play.Info;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class PlayerGameViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayerGameViewModel(IPlayerGameFront front)
        {
            Game = front.Game;

            Time = front.Game
                .CombineLatest(
                    front.Cycles,
                    (game, cycles) => game is null
                        ? TimeSpan.MinValue
                        : game.Value.Mode == GameMode.Challenge
                            ? TimeSpan.FromSeconds(MathF.Ceiling(PlayConst.ChallengeSeconds - cycles))
                            : TimeSpan.FromSeconds(cycles))
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            TimeNormalized = front.Game
                .CombineLatest(
                    front.Cycles,
                    (game, cycles) => game is null
                        ? 0
                        : game.Value.Mode == GameMode.Challenge
                            ? Math.Clamp(1 - (cycles / PlayConst.ChallengeSeconds), 0, 1)
                            : 0)
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);
        }

        public ReadOnlyReactiveProperty<Game?> Game { get; }

        public ReadOnlyReactiveProperty<TimeSpan> Time { get; }

        public ReadOnlyReactiveProperty<float> TimeNormalized { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
