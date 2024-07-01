using System.Collections.Generic;
using Some1.Play.Info;

namespace Some1.Play.Front
{
    public interface IPlayerObjectCastFront
    {
        IReadOnlyDictionary<CastId, IPlayerObjectCastItemFront> Items { get; }
    }
}
