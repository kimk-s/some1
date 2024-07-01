using System;

namespace Some1.Play.Info
{
    [Flags]
    public enum TeamTargetInfo : byte
    {
        None = 0,
        Neutral = 1,
        Ally = 2,
        Enemy = 4,
        All = Neutral | Ally | Enemy,
    }
}
