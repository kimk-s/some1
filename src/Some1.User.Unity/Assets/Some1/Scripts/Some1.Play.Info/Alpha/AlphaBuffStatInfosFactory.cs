using System.Collections.Generic;

namespace Some1.Play.Info.Alpha
{
    public static class AlphaBuffStatInfosFactory
    {
        public static IEnumerable<BuffStatInfo> Create() => new BuffStatInfo[]
        {
            new(
                BuffId.Buff1,
                Trait.All,
                StatId.Defense,
                7),
            new(
                BuffId.Buff3,
                Trait.All,
                StatId.Accel,
                -5),
            new(
                BuffId.Buff5,
                Trait.All,
                StatId.Defense,
                7),
            new(
                BuffId.Buff6,
                Trait.All,
                StatId.Defense,
                9),
            new(
                BuffId.Buff8,
                Trait.All,
                StatId.StunCast,
                1),
            new(
                BuffId.Buff8,
                Trait.All,
                StatId.StunWalk,
                1),
            new(
                BuffId.Buff9,
                Trait.All,
                StatId.Accel,
                9),
        };
    }
}
