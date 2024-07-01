using System;
using Some1.Play.Info;

namespace Some1.Play.Core
{
    public readonly struct CycleSpan
    {
        public CycleSpan(float a, float b)
        {
            if (a < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(a));
            }

            if (b < a)
            {
                throw new ArgumentOutOfRangeException(nameof(b));
            }

            A = a;
            B = b;
        }

        public float A { get; }

        public float B { get; }

        public float Length => B - A;

        public int Contains(CycleRepeat repeat) => Contains(repeat.Start, repeat.Interval);

        public int Contains(float start, float interval)
        {
            if (start < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            if (A == B)
            {
                return 0;
            }

            if (interval == CycleRepeat.EveryFrameInterval)
            {
                return start < B ? 1 : 0;
            }

            if (interval < 0)
            {
                return start >= A && start < B ? 1 : 0;
            }

            var i = start >= A ? 0 : Math.Max((int)MathF.Floor((A - start) / interval) - 1, 0);
            var result = 0;
            for (; ; i++)
            {
                var x = MathF.Round(start + interval * i, 3);
                if (x >= B)
                {
                    break;
                }
                if (x >= A)
                {
                    result++;
                }
            }
            return result;
        }
    }
}
