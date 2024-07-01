using System;
using R3;

namespace Some1.Sync
{
    public interface ISyncDestination : ISyncReadable, IDisposable
    {
        ReadOnlyReactiveProperty<bool> IsDefault { get; }

        void Reset();
    }
}
