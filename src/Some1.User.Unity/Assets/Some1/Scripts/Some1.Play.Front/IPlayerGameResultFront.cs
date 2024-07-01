using System;
using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IPlayerGameResultFront
    {
        ReadOnlyReactiveProperty<GameResult?> Result { get; }
        ReadOnlyReactiveProperty<TimeSpan> EndTimeAgo { get; }
    }
}
