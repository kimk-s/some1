using System;
using R3;

namespace Some1.Sync
{
    public interface ISyncSource : ISyncWritable, IDisposable
    {
        ReadOnlyReactiveProperty<bool> IsDefault { get; }
        ReadOnlyReactiveProperty<bool> Dirty { get; }

        void ClearDirty();
    }
}
