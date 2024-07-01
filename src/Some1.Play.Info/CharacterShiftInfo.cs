using System;

namespace Some1.Play.Info
{
    public sealed class CharacterShiftInfo
    {
        public CharacterShiftInfo(float? jumpCycle, float? knockCycle, float? dashCycle, float? grabCycle)
        {
            if (jumpCycle is not null && jumpCycle.Value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(jumpCycle));
            }

            if (knockCycle is not null && knockCycle.Value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(knockCycle));
            }

            if (dashCycle is not null && dashCycle.Value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(dashCycle));
            }

            if (grabCycle is not null && grabCycle.Value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(grabCycle));
            }

            JumpCycle = jumpCycle;
            KnockCycle = knockCycle;
            DashCycle = dashCycle;
            GrabCycle = grabCycle;
        }

        public float? JumpCycle { get; }

        public float? KnockCycle { get; }

        public float? DashCycle { get; }

        public float? GrabCycle { get; }
    }
}
