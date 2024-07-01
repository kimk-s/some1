using System;

namespace Some1.Play.Info
{
    public sealed class CharacterAreaInfo
    {
        public CharacterAreaInfo(AreaType type, float size)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            Type = type;
            Size = size;
        }

        public AreaType Type { get; }

        public float Size { get; }
    }
}
