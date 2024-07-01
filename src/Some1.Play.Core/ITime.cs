using System;
using Some1.Sync;

namespace Some1.Play.Core
{
    public interface ITime : ISyncTime
    {
        float DeltaSeconds { get; }
        double TotalSeconds { get; }
        double UtcNowSeconds { get; }
        DateTime UtcNow { get; }
    }
}
