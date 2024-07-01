namespace Some1.Play.Info
{
    public enum ShiftId : byte
    {
        Jump,
        Knock,
        Dash,
        Grab,
    }

    public static class ShiftIdExtensions
    {
        public static float GetTransformRotation(this ShiftId id, float rotation) => id switch
        {
            ShiftId.Knock => rotation + 180,
            _ => rotation
        };

        public static float GetMaxHeight(this ShiftId id) => id switch
        {
            ShiftId.Jump => 5,
            ShiftId.Knock => 1,
            _ => 0
        };

        public static bool IsStopCast(this ShiftId id) => id switch
        {
            ShiftId.Dash => false,
            _ => true
        };

        public static BumpLevel GetBumpLevel(this ShiftId id) => id switch
        {
            ShiftId.Dash => BumpLevel.Middle,
            _ => BumpLevel.High
        };

        public static bool CanAddHitAndBuff(this ShiftId id) => id switch
        {
            ShiftId.Knock => true,
            _ => false
        };
    }
}
