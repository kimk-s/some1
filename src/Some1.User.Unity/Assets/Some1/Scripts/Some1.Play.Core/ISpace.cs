using System.Collections.Generic;
using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface ISpace
    {
        SpaceInfo Info { get; }
        Area Area { get; }
        int BlockCount { get; }
        IEnumerable<IBlock<IObject>> Blocks { get; }

        IBlock<IObject> GetBlock(BlockId id);
        IBlock<IObject>[] GetBlocks(Area area);
        IObject[] GetObjects(Area area);
    }
}
