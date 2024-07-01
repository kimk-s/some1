using System.Collections.Generic;

namespace Some1.Play.Core
{
    public interface IPlayerGroup
    {
        IReadOnlyList<IPlayer> All { get; }
        int UidCount { get; }
        int NonUidCount { get; }

        IEnumerable<IPlayer> GetUidPlayers();
    }
}
