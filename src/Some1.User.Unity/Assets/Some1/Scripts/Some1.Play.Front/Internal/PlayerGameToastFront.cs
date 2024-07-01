using System;
using System.Linq;
using Some1.Play.Info;
using R3;

namespace Some1.Play.Front.Internal
{
    public sealed class PlayerGameToastFront : IPlayerGameToastFront
    {
        public PlayerGameToastFront(
            IPlayerGameManagerFront manager,
            IPlayerGameArgsGroupFront argses,
            IPlayerGameFront game,
            IPlayerGameResultGroupFront results,
            ITimeFront time)
        {
            State = manager.Status
                .CombineLatest(
                    manager.Cycles.Select(x => Math.Min((int)MathF.Floor(x), PlayConst.PlayerGameManagerReturnSeconds)).ToReadOnlyReactiveProperty(),
                    (status, cycles) => (status, cycles))
                .CombineLatest(
                    argses.Selected,
                    game.Game.CombineLatest(
                        game.Cycles.Select(x => Math.Min((int)MathF.Floor(x), 5)).ToReadOnlyReactiveProperty(),
                        (game, cycles) => (game, cycles)),
                    results.Latest.Select(x => x?.Result.CurrentValue).ToReadOnlyReactiveProperty(),
                    time.UtcNow,
                    (man, args, game, result, utcNow) =>
                    {
                        if (man.status is not null)
                        {
                            return new PlayerGameToastState(
                                man.status switch
                                {
                                    GameManagerStatus.Ready => PlayerGameToastStateType.Ready,
                                    GameManagerStatus.Return => PlayerGameToastStateType.Return,
                                    GameManagerStatus.ReReady => PlayerGameToastStateType.ReReady,
                                    GameManagerStatus.ReturnFaulted => PlayerGameToastStateType.ReturnFaulted,
                                    _ => throw new InvalidOperationException()
                                },
                                args?.Id,
                                man.cycles,
                                0,
                                false);
                        }
                        else if (game.game is not null)
                        {
                            return game.cycles < 5
                                ? new PlayerGameToastState(
                                    PlayerGameToastStateType.Start,
                                    game.game.Value.Mode,
                                    game.cycles,
                                    0,
                                    false)
                                : new();
                        }
                        else if (result is not null)
                        {
                            int time = (int)Math.Floor((utcNow - result.Value.EndTime).TotalSeconds);
                            return time < 5
                                ? new PlayerGameToastState(
                                    PlayerGameToastStateType.End,
                                    result.Value.Game.Mode,
                                    time,
                                    result.Value.Game.Score,
                                    result.Value.Success)
                                : new();
                        }

                        return new();
                    })
                .ToReadOnlyReactiveProperty();
        }

        public ReadOnlyReactiveProperty<PlayerGameToastState> State { get; }
    }
}
