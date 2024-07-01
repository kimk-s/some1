using System;

namespace Some1.Play.Info
{
    public sealed class BuffInfo
    {
        public BuffInfo(
            BuffId id,
            float cycle,
            bool isUnique,
            BuffTraitInfo? trait = null,
            BuffHitInfo? hit = null)
        {
            if (cycle <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cycle));
            }

            Id = id;
            Cycle = cycle;
            IsUnique = isUnique;
            Trait = trait;
            Hit = hit;
        }

        public BuffId Id { get; }

        public string Name { get; set; } = string.Empty;

        public float Cycle { get; }

        public bool IsUnique { get; }

        public BuffTraitInfo? Trait { get; }

        public BuffHitInfo? Hit { get; }
    }
}
