using System.Collections.Generic;

namespace Some1.Play.Info.Alpha
{
    public static class AlphaBuffInfosFactory
    {
        public static IEnumerable<BuffInfo> Create() => new BuffInfo[]
        {
            new(
                BuffId.Buff1,
                6,
                true),
            new(
                BuffId.Buff2,
                4,
                true,
                hit : new(
                    Trait.All,
                    HitId.Default,
                    55,
                    0)),
            new(
                BuffId.Buff3,
                3,
                true,
                hit: new(
                    Trait.All,
                    HitId.Default,
                    100,
                    0)),
            new(
                BuffId.Buff4,
                3,
                true,
                hit: new(
                    Trait.All,
                    HitId.Recovery,
                    1000,
                    0)),
            new(
                BuffId.Buff5,
                10,
                true),
            new(
                BuffId.Buff6,
                10,
                true),
            new(
                BuffId.Buff7,
                10,
                true,
                hit : new(
                    Trait.All,
                    HitId.Default,
                    50,
                    0)),
            new(
                BuffId.Buff8,
                3,
                true,
                hit : new(
                    Trait.All,
                    HitId.Default,
                    150,
                    0)),
            new(
                BuffId.Buff9,
                10,
                true),
            new(
                BuffId.Buff10,
                4,
                true),
            new(
                BuffId.Buff11,
                4,
                true),
            new(
                BuffId.Buff12,
                10,
                true),
        };
    }
}
