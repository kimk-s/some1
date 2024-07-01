using System;
using System.Numerics;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal static class SnapHelper
    {
        internal static Area SnapBackX(Area area, Vector2 delta)
        {
            area.Position += new Vector2(GetBackAdjustX(area, delta), 0);
            return area;
        }

        internal static Area SnapBackY(Area area, Vector2 delta)
        {
            area.Position += new Vector2(0, GetBackAdjustY(area, delta));
            return area;
        }

        internal static Area SnapBackXY(Area area, Vector2 delta)
        {
            area.Position += new Vector2(GetBackAdjustX(area, delta), GetBackAdjustY(area, delta));
            return area;
        }

        private static float GetBackAdjustX(Area area, Vector2 delta)
        {
            if (delta.X > 0)
            {
                return MathF.Floor(area.Right) - area.Right;
            }
            else if (delta.X < 0)
            {
                return MathF.Ceiling(area.Left) - area.Left;
            }
            else
            {
                return 0;
            }
        }

        private static float GetBackAdjustY(Area area, Vector2 delta)
        {
            if (delta.Y > 0)
            {
                return MathF.Floor(area.Bottom) - area.Bottom;
            }
            else if (delta.Y < 0)
            {
                return MathF.Ceiling(area.Top) - area.Top;
            }
            else
            {
                return 0;
            }
        }

        internal static Area SnapGoX(Area area, Vector2 delta)
        {
            area.Position += new Vector2(GetGoAdjustX(area, delta), 0);
            return area;
        }

        internal static Area SnapGoY(Area area, Vector2 delta)
        {
            area.Position += new Vector2(0, GetGoAdjustY(area, delta));
            return area;
        }

        internal static Area SnapGoXY(Area area, Vector2 delta)
        {
            area.Position += new Vector2(GetGoAdjustX(area, delta), GetGoAdjustY(area, delta));
            return area;
        }

        private static float GetGoAdjustX(Area area, Vector2 delta)
        {
            if (delta.X > 0)
            {
                return MathF.Ceiling(area.Right) - area.Right + 1;
            }
            else
            {
                return MathF.Floor(area.Left) - area.Left - 1;
            }
        }

        private static float GetGoAdjustY(Area area, Vector2 delta)
        {
            if (delta.Y > 0)
            {
                return MathF.Ceiling(area.Bottom) - area.Bottom + 1;
            }
            else
            {
                return MathF.Floor(area.Top) - area.Top - 1;
            }
        }
    }
}
