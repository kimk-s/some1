namespace Some1.Play.Info
{
    public static class AimHelper
    {
        public static Aim From(TargetAimInfo info, Aim aim)
        {
            return new(info.Rotation.Mix(aim.Rotation), info.Length.Mix(aim.Length));
        }
    }
}
