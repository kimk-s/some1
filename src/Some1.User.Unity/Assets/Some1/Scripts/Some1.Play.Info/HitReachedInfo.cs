using System;

namespace Some1.Play.Info
{
    public sealed class HitReachedInfo
    {
        public HitReachedInfo(HitAttribute attribute, int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            Attribute = attribute;
            Value = value;
        }

        public HitAttribute Attribute { get; }
        public int Value { get; }
    }
}
