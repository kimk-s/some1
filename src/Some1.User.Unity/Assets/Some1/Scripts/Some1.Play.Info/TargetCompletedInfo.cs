using System;

namespace Some1.Play.Info
{
    public readonly struct TargetCompletedInfo
    {
        public TargetCompletedInfo(bool keepTargetToken, int healthDecreasement)
        {
            if (healthDecreasement < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(healthDecreasement));
            }

            KeepTargetToken = keepTargetToken;
            HealthDecreasement = healthDecreasement;
        }

        public bool KeepTargetToken { get; }

        public int HealthDecreasement { get; }
    }
}
