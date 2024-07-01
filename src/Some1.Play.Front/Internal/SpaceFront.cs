using System.Drawing;
using Some1.Play.Info;

namespace Some1.Play.Front.Internal
{
    internal sealed class SpaceFront : ISpaceFront
    {
        public SpaceFront(SpaceInfo info)
        {
            Area = Area.Rectangle(PointF.Empty, info.SpaceTiles);
        }

        public Area Area { get; }
    }
}
