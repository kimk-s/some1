using System;
using R3;

namespace Some1.Play.Front
{
    public interface IPlayerExpFront
    {
        ReadOnlyReactiveProperty<int> Value { get; }
        ReadOnlyReactiveProperty<DateTime> ChargeTime { get; }
        ReadOnlyReactiveProperty<TimeSpan> ChargeTimeRemained { get; }
    }
}
