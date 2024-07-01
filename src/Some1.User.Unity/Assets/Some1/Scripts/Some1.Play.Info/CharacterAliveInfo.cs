using System;

namespace Some1.Play.Info
{
    public sealed class CharacterAliveInfo
    {
        public CharacterAliveInfo(CharacterAliveId id, float cycle)
        {
            if (cycle <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cycle));
            }

            Id = id;
            Cycle = cycle;
        }

        public CharacterAliveId Id { get; }

        public float Cycle { get; }
    }
}
