using System.Numerics;

namespace Some1.Play.Core
{
    public interface IObjectTransfer
    {
        Vector2 LastWarpPosition { get; }
        bool IsMoveBlocked { get; }
        Vector2 MoveDelta { get; }
        IObjectTransferMoveFailed MoveFailed { get; }
    }
}
