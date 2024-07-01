using System;

namespace Some1.Play.Info
{
    [Flags]
    public enum AimTeamInfo
    {
        None = 0,
        Neutral = 1,
        Ally = 2,
        Enemy = 4,
    }
}
