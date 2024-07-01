using System.Collections.Generic;
using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IBlock<out T>
    {
        BlockId Id { get; }
        Area Area { get; }
        IEnumerable<T> Items { get; }
        IControl Control { get; }
    }
}
