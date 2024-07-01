using System.Numerics;

namespace Some1.Play.Info
{
    public static class PositionHelper
    {
        public static Vector2 From(TargetAimInfo info, Aim aim, Vector2 position)
        {
            return AimHelper.From(info, aim).ToVector2() + position;
        }
    }
}
