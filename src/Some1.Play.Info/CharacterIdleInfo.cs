using System;

namespace Some1.Play.Info
{
    public sealed class CharacterIdleInfo
    {
        public CharacterIdleInfo(CharacterIdleId id, float cycle)
        {
            if (cycle <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cycle));
            }

            Id = id;
            Cycle = cycle;
        }

        public CharacterIdleId Id { get; }

        public float Cycle { get; }
    }
}
