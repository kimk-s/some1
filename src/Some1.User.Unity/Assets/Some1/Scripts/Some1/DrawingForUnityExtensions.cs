using System.Drawing;
using System.Numerics;

namespace Some1
{
    public static class DrawingForUnityExtensions
    {
        public static Vector2 ToVector2(this PointF that)
        {
#if UNITY
            return new(that.X, that.Y);
#else
            return (Vector2)that;
#endif
        }

        public static Vector2 ToVector2(this SizeF that)
        {
#if UNITY
            return new(that.Width, that.Height);
#else
            return (Vector2)that;
#endif
        }

        public static PointF ToPointF(this Vector2 that)
        {
#if UNITY
            return new(that.X, that.Y);
#else
            return (PointF)that;
#endif
        }

        public static SizeF ToSizeF(this Vector2 that)
        {
#if UNITY
            return new(that.X, that.Y);
#else
            return (SizeF)that;
#endif
        }
    }
}
