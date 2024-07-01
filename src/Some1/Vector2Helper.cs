using System;
using System.Numerics;

namespace Some1
{
    public static class Vector2Helper
    {
        public const float Deg2Rad = (float)(Math.PI / 180);
        public const float Rad2Deg = (float)(180 / Math.PI);

        public static float From0To360(float angle)
        {
            if (angle >= 360)
            {
                return angle % 360;
            }
            else if (angle < 0)
            {
                return (angle % 360) + 360;
            }
            else
            {
                return angle;
            }
        }

        public static Vector2 Normalize(float angle)
        {
            return new((float)MathF.Cos(angle * Deg2Rad), (float)MathF.Sin(angle * Deg2Rad));
        }

        public static float Angle(Vector2 a)
        {
            return AngleBetween(Vector2.Zero, a);
        }

        public static float AngleBetween(Vector2 a, Vector2 b)
        {
            return Convert180To360Degrees((float)MathF.Atan2(b.Y - a.Y, b.X - a.X) * Rad2Deg);
        }

        private static float Convert180To360Degrees(float _180Degrees)
        {
            return (_180Degrees + 360) % 360;
        }
    }
}
