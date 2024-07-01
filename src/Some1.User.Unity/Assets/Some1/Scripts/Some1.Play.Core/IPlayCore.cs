using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Some1.Play.Core
{
    public interface IPlayCore
    {
        IReadOnlyList<ILeader> Leaders { get; }
        IPlayerGroup Players { get; }
        IEnumerable<INonPlayer> NonPlayers { get; }
        IRegionGroup Regions { get; }
        ISpace Space { get; }
        ITime Time { get; }

        ValueTask AddUidPipeAsync(UidPipe authPipe, CancellationToken cancellationToken);
        void Update(float deltaSeconds);
    }
}
