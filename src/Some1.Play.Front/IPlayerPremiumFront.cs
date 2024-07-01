using System;
using R3;

namespace Some1.Play.Front
{
    public interface IPlayerPremiumFront
    {
        ReadOnlyReactiveProperty<bool> IsPremium { get; }
        ReadOnlyReactiveProperty<DateTime> EndTime { get; }
        ReadOnlyReactiveProperty<TimeSpan> TimeLeft { get; }
    }
}
