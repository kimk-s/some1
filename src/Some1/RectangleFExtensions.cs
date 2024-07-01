using System.Drawing;

namespace Some1
{
    public static class RectangleFExtensions
    {
        public static PointF Center(this RectangleF rect)
            => new(rect.Location.X + rect.Size.Width * 0.5f, rect.Location.Y + rect.Size.Height * 0.5f);
    }
}
