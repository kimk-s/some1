using System.Collections.Generic;

namespace Some1.Play.Info.Alpha
{
    public static class AlphaCharacterIdleInfosFactory
    {
        public static IEnumerable<CharacterIdleInfo> Create() => new CharacterIdleInfo[]
        {
            new(new(CharacterId.Player1, true), 1f),
            new(new(CharacterId.Player2, true), 5f),
            new(new(CharacterId.Player3, true), 5f),
            new(new(CharacterId.Player4, true), 1f),
            new(new(CharacterId.Player5, true), 4f),
            new(new(CharacterId.Player6, true), 5f),

            new(new(CharacterId.Mob1, true), 1f),
            new(new(CharacterId.Mob2, true), 1f),
            new(new(CharacterId.Mob3, true), 1f),
            new(new(CharacterId.Mob4, true), 1f),

            new(new(CharacterId.Chief1, true), 1f),
            new(new(CharacterId.Chief2, true), 1f),

            new(new(CharacterId.Boss1, true), 1f),
            new(new(CharacterId.Boss2, true), 1f),
            new(new(CharacterId.Boss3, true), 1f),
            new(new(CharacterId.Boss4, true), 1f),
            new(new(CharacterId.Boss5, true), 1f),
            new(new(CharacterId.Boss6, true), 1f),

            new(new(CharacterId.Animal1, true), 1f),
            new(new(CharacterId.Animal2, true), 1f),
            new(new(CharacterId.Animal3, true), 1f),
            new(new(CharacterId.Animal4, true), 1f),
            new(new(CharacterId.Animal5, true), 1f),

            new(new(CharacterId.Summon1, true), 1f),
        };
    }
}
