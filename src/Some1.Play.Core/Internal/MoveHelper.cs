using System.Numerics;

namespace Some1.Play.Core.Internal
{
    internal static class MoveHelper
    {
        public static Vector2 CalculateDistance(float rotation, float speed, float time)
        {
            return CalculateDistance(CalculateVelocity(rotation, speed), time);
        }

        public static Vector2 CalculateDistance(Vector2 velocity, float time)
        {
            return velocity * time;
        }

        public static Vector2 CalculateVelocity(float rotation, float speed)
        {
            return Vector2Helper.Normalize(rotation) * speed;
        }
    }
}
