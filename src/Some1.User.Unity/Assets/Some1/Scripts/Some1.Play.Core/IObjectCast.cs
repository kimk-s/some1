using System.Collections.Generic;
using R3;
using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IObjectCast
    {
        IReadOnlyDictionary<CastId, IObjectCastItem> Items { get; }
        Cast? Next { get; }
        ReadOnlyReactiveProperty<Cast?> Current { get; }
        IObjectCycles Cycles { get; }
    }
}
