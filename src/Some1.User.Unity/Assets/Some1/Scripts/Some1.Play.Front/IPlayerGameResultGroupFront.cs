using System.Collections.Generic;
using R3;

namespace Some1.Play.Front
{
    public interface IPlayerGameResultGroupFront
    {
        IReadOnlyList<IPlayerGameResultFront> All { get; }
        ReadOnlyReactiveProperty<IPlayerGameResultFront?> Latest { get; }
    }
}
