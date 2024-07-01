using System;

namespace Some1.Play.Info
{
    public sealed class CharacterWalkInfo
    {
        public CharacterWalkInfo(float cycle, float speed)
        {
            if (cycle <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cycle));
            }

            if (speed <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(speed));
            }

            Cycle = cycle;
            Speed = speed;
        }

        public float Cycle { get; }

        public float Speed { get; }
    }
}
