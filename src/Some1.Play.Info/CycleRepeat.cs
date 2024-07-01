using System;

namespace Some1.Play.Info
{
    public sealed class CycleRepeat
    {
        public const float EveryFrameInterval = 0;

        public const float NoRepeatInterval = -1;

        public CycleRepeat(float start, float interval)
        {
            if (start < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            Start = start;
            Interval = interval;
        }

        public float Start { get; }

        public float Interval { get; }
    }
}
