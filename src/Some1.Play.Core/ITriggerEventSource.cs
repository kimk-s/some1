using System;
using Some1.Play.Core.Paralleling;

namespace Some1.Play.Core
{
    public interface ITriggerEventSource<TEventArgs>
        where TEventArgs : struct
    {
        event EventHandler<(TEventArgs e, ParallelToken parallelToken)>? EventFired;
        event EventHandler? ScopedReset;
    }

    public readonly struct EmptyTriggerEventArgs
    {
    }

    public readonly struct PostTriggerEventArgs
    {

    }
}
