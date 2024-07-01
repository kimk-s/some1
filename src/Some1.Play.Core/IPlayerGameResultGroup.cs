using System.Collections.Generic;

namespace Some1.Play.Core
{
    public interface IPlayerGameResultGroup
    {
        IReadOnlyList<IPlayerGameResult> All { get; }
    }
}
