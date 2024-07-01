using System.Collections.Generic;
using R3;

namespace Some1.Play.Front
{
    public interface IPlayerObjectSpecialtyGroupFront
    {
        IReadOnlyList<IPlayerObjectSpecialtyFront> All { get; }
        ReadOnlyReactiveProperty<int> ActiveCount { get; }
        int PinnedCount { get; }
    }
}
