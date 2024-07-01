using System;
using R3;
using Some1.Sync;

namespace Some1.Play.Front
{
    public interface ITimeFront : ISyncTime
    {
        new ReadOnlyReactiveProperty<int> FrameCount { get; }
        ReadOnlyReactiveProperty<DoubleWave> TotalSecondsWave { get; }
        ReadOnlyReactiveProperty<double> TotalSeconds { get; }
        ReadOnlyReactiveProperty<double> UtcNowSeconds { get; }
        ReadOnlyReactiveProperty<DateTime> UtcNow { get; }
    }
}
