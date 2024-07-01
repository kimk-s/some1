using System.Collections.Generic;
using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IPlayerGameArgsGroup
    {
        IReadOnlyDictionary<GameMode, IPlayerGameArgs> All { get; }
        IPlayerGameArgs Selected { get; }
    }
}
