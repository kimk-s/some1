using System.Collections.Generic;

namespace Some1.Play.Info.Alpha
{
    public static class AlphaBoosterInfoFactory
    {
        public static IEnumerable<BoosterInfo> Create() => new BoosterInfo[]
        {
            new(BoosterId.Power, 180),
        };
    }
}
