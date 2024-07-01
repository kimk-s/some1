using System;

namespace Some1.Play.Core.Internal
{
    internal static class SpeedHelper
    {
        private static float Tolerance = 0.0001f;

        internal static float GetSpeed(float distance, float time) => time == 0 ? 0 : distance / time;

        internal static float GetTime(float distance, float speed) => speed == 0 ? 0 : distance / speed;

        internal static float GetDistance(float time, float speed) => time * speed;

        internal static bool IsValid(float distance, float time, float speed) => Math.Abs(GetSpeed(distance, time) - speed) <= Tolerance;
    }
}
