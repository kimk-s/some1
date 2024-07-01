namespace Some1.Play.Info
{
    public enum WalkSpeed
    {
        VerySlow,
        Slow,
        Normal,
        Fast,
        VeryFast,
    }

    public static class WalkSpeedHelper
    {
        public static WalkSpeed Get(float walkSpeed) => walkSpeed switch
        {
            >= 2.73f => WalkSpeed.VeryFast,
            >= 2.57f => WalkSpeed.Fast,
            >= 2.4f => WalkSpeed.Normal,
            _ => WalkSpeed.Slow,
        };
    }
}
