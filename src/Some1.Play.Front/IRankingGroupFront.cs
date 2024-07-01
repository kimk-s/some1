using System.Collections.Generic;
using R3;

namespace Some1.Play.Front
{
    public interface IRankingGroupFront
    {
        IReadOnlyList<IRankingFront> All { get; }
        ReadOnlyReactiveProperty<IRankingFront?> Mine { get; }
        ReadOnlyReactiveProperty<int> TimeLeftUntilUpdate { get; }
    }
}
