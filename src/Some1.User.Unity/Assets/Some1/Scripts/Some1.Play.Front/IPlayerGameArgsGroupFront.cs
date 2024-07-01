using System.Collections.Generic;
using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IPlayerGameArgsGroupFront
    {
        IReadOnlyDictionary<GameMode, IPlayerGameArgsFront> All { get; }
        ReadOnlyReactiveProperty<IPlayerGameArgsFront?> Selected { get; }
    }
}
