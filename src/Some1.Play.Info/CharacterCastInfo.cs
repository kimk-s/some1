using System;

namespace Some1.Play.Info
{
    public sealed class CharacterCastInfo
    {
        public CharacterCastInfo(
            CharacterCastId id,
            float cycle,
            float loadTime,
            int loadCount,
            bool useOn,
            CharacterCastAimLimitInfo aimLimit,
            CastAimInfo aim,
            CharacterCastTraitInfo? trait = null)
        {
            if (cycle <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cycle));
            }

            if (loadTime <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(loadTime));
            }

            if (loadCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(loadCount));
            }

            if (id.Cast != CastId.Attack && loadCount != 1)
            {
                throw new ArgumentException("LoadCount must be 1 when CastId is not Attack", nameof(loadCount));
            }

            Id = id;
            Cycle = cycle;
            LoadTime = loadTime;
            LoadCount = loadCount;
            UseOn = useOn;
            AimLimit = aimLimit;
            Aim = aim;
            Trait = trait;
        }

        public CharacterCastId Id { get; }

        public float Cycle { get; }

        public float LoadTime { get; }

        public int LoadCount { get; }

        public bool UseOn { get; }

        public CharacterCastAimLimitInfo AimLimit { get; }

        public CastAimInfo Aim { get; }

        public CharacterCastTraitInfo? Trait { get; }
    }
}
