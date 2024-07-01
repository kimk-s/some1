using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectBlock : Block<Object, ObjectStatic>
    {
        private readonly BlockTiles _blockTiles;

        internal ObjectBlock(BlockId id, Area area, ParallelOptions parallelOptions, ITime time, BlockTiles blockTiles)
            : base(id, area, parallelOptions, time)
        {
            _blockTiles = blockTiles;
        }

        protected override bool Add(Object itemValue)
        {
            bool result = base.Add(itemValue);
            if (result)
            {
                _blockTiles.Add(itemValue);
            }
            return result;
        }

        protected override bool Remove(Object itemValue)
        {
            bool result = base.Remove(itemValue);
            if (result)
            {
                _blockTiles.Remove(itemValue);
            }
            return result;
        }
    }
}
