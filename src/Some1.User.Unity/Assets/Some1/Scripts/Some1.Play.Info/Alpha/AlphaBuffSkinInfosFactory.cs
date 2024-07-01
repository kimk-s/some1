using System.Collections.Generic;

namespace Some1.Play.Info.Alpha
{
    public static class AlphaBuffSkinInfosFactory
    {
        public static IEnumerable<BuffSkinInfo> Create() => new BuffSkinInfo[]
        {
            new(new(BuffId.Buff1, SkinId.Skin0)),
            new(new(BuffId.Buff1, SkinId.Skin1)),
            new(new(BuffId.Buff1, SkinId.Skin2)),
            new(new(BuffId.Buff1, SkinId.Skin3)),
            new(new(BuffId.Buff1, SkinId.Skin4)),
            new(new(BuffId.Buff1, SkinId.Skin5)),

            new(new(BuffId.Buff2, SkinId.Skin0)),
            new(new(BuffId.Buff3, SkinId.Skin0)),
            new(new(BuffId.Buff4, SkinId.Skin0)),
            new(new(BuffId.Buff5, SkinId.Skin0)),
            new(new(BuffId.Buff6, SkinId.Skin0)),
            new(new(BuffId.Buff7, SkinId.Skin0)),
            new(new(BuffId.Buff8, SkinId.Skin0)),
            new(new(BuffId.Buff9, SkinId.Skin0)),
            new(new(BuffId.Buff10, SkinId.Skin0)),
            new(new(BuffId.Buff11, SkinId.Skin0)),
            new(new(BuffId.Buff12, SkinId.Skin0)),
        };
    }
}
