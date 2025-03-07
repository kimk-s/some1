﻿using System.Collections.Generic;

namespace Some1.Play.Info.Alpha
{
    public static class AlphaCharacterAliveInfosFactory
    {
        public static IEnumerable<CharacterAliveInfo> Create() => new CharacterAliveInfo[]
        {
            new(new(CharacterId.Player1, false), 2),
            new(new(CharacterId.Player1, true), 1),
            new(new(CharacterId.Player2, false), 2),
            new(new(CharacterId.Player2, true), 1),
            new(new(CharacterId.Player3, false), 2),
            new(new(CharacterId.Player3, true), 1),
            new(new(CharacterId.Player4, false), 2),
            new(new(CharacterId.Player4, true), 2),
            new(new(CharacterId.Player5, false), 1),
            new(new(CharacterId.Player5, true), 1),
            new(new(CharacterId.Player6, false), 2),
            new(new(CharacterId.Player6, true), 1),

            new(new(CharacterId.Mob1, false), 2),
            new(new(CharacterId.Mob1, true), 1),
            new(new(CharacterId.Mob2, false), 2),
            new(new(CharacterId.Mob2, true), 1),
            new(new(CharacterId.Mob3, false), 2),
            new(new(CharacterId.Mob3, true), 1),
            new(new(CharacterId.Mob4, false), 2),
            new(new(CharacterId.Mob4, true), 1),

            new(new(CharacterId.Chief1, false), 2),
            new(new(CharacterId.Chief1, true), 1),
            new(new(CharacterId.Chief2, false), 2),
            new(new(CharacterId.Chief2, true), 1),

            new(new(CharacterId.Boss1, false), 2),
            new(new(CharacterId.Boss1, true), 1),
            new(new(CharacterId.Boss2, false), 2),
            new(new(CharacterId.Boss2, true), 1),
            new(new(CharacterId.Boss3, false), 2),
            new(new(CharacterId.Boss3, true), 1),
            new(new(CharacterId.Boss4, false), 2),
            new(new(CharacterId.Boss4, true), 1),
            new(new(CharacterId.Boss5, false), 2),
            new(new(CharacterId.Boss5, true), 1),
            new(new(CharacterId.Boss6, false), 2),
            new(new(CharacterId.Boss6, true), 1),

            new(new(CharacterId.Plant1, false), 1),
            new(new(CharacterId.Plant2, false), 1),
            new(new(CharacterId.Plant3, false), 1),
            new(new(CharacterId.Plant4, false), 1),
            new(new(CharacterId.Plant5, false), 1),
            new(new(CharacterId.Plant6, false), 1),
            new(new(CharacterId.Plant7, false), 1),
            new(new(CharacterId.Plant8, false), 1),

            new(new(CharacterId.Animal1, false), 2),
            new(new(CharacterId.Animal1, true), 1),
            new(new(CharacterId.Animal2, false), 2),
            new(new(CharacterId.Animal2, true), 1),
            new(new(CharacterId.Animal3, false), 2),
            new(new(CharacterId.Animal3, true), 1),
            new(new(CharacterId.Animal4, false), 2),
            new(new(CharacterId.Animal4, true), 1),
            new(new(CharacterId.Animal5, false), 2),
            new(new(CharacterId.Animal5, true), 1),

            new(new(CharacterId.Summon1, false), 2),
            new(new(CharacterId.Summon1, true), 1),

            new(new(CharacterId.Box, false), 1),

            new(new(CharacterId.Missile1, true), 1),
            new(new(CharacterId.Missile2, true), 1),
            new(new(CharacterId.Missile3, true), 1),
            new(new(CharacterId.Missile4, true), 1),
            new(new(CharacterId.Missile5, true), 1),
            new(new(CharacterId.Missile6, true), 1),
            new(new(CharacterId.Missile7, true), 1),
            new(new(CharacterId.Missile8, true), 1),
            new(new(CharacterId.Missile9, true), 1),
            new(new(CharacterId.Missile10, true), 1),
            new(new(CharacterId.Missile11, true), 1),
            new(new(CharacterId.Missile12, true), 1),
            new(new(CharacterId.Missile13, true), 1),
            new(new(CharacterId.Missile14, true), 1),
            new(new(CharacterId.Missile15, true), 1),
            new(new(CharacterId.Missile16, true), 1),
            new(new(CharacterId.Missile17, true), 1),
            new(new(CharacterId.Missile18, true), 1),
            new(new(CharacterId.Missile19, true), 1),
            new(new(CharacterId.Missile20, true), 1),
            new(new(CharacterId.Missile21, true), 1),
            new(new(CharacterId.Missile22, true), 1),
            new(new(CharacterId.Missile23, true), 1),
            new(new(CharacterId.Missile24, true), 1),
            new(new(CharacterId.Missile25, true), 1),
            new(new(CharacterId.Missile26, true), 1),
            new(new(CharacterId.Missile27, true), 1),
            new(new(CharacterId.Missile28, true), 1),
            new(new(CharacterId.Missile29, true), 1),
            new(new(CharacterId.Missile30, true), 1),
            new(new(CharacterId.Missile31, true), 1),
            new(new(CharacterId.Missile32, true), 1),

            new(new(CharacterId.Explosion1, true), 1),
            new(new(CharacterId.Explosion2, true), 1),
            new(new(CharacterId.Explosion3, true), 1),
            new(new(CharacterId.Explosion4, true), 1),
            new(new(CharacterId.Explosion5, true), 1),
            new(new(CharacterId.Explosion6, true), 1),
            new(new(CharacterId.Explosion7, true), 1),
            new(new(CharacterId.Explosion8, true), 1),
            new(new(CharacterId.Explosion9, true), 1),
            new(new(CharacterId.Explosion10, true), 1),
            new(new(CharacterId.Explosion11, true), 1),
            new(new(CharacterId.Explosion12, true), 1),
            new(new(CharacterId.Explosion13, true), 1),
            new(new(CharacterId.Explosion14, true), 1),
            new(new(CharacterId.Explosion15, true), 1),
            new(new(CharacterId.Explosion16, true), 1),
            new(new(CharacterId.Explosion17, true), 1),
            new(new(CharacterId.Explosion18, true), 1),
            new(new(CharacterId.Explosion19, true), 1),
            new(new(CharacterId.Explosion20, true), 1),
            new(new(CharacterId.Explosion21, true), 1),
            new(new(CharacterId.Explosion22, true), 1),
            new(new(CharacterId.Explosion23, true), 1),
            new(new(CharacterId.Explosion24, true), 1),
            new(new(CharacterId.Explosion25, true), 1),
            new(new(CharacterId.Explosion26, true), 1),
            new(new(CharacterId.Explosion27, true), 1),
            new(new(CharacterId.Explosion28, true), 1),
            new(new(CharacterId.Explosion29, true), 1),
            new(new(CharacterId.Explosion30, true), 1),
            new(new(CharacterId.Explosion31, true), 1),
            new(new(CharacterId.Explosion32, true), 1),

            new(new(CharacterId.Magic1, true), 1),
            new(new(CharacterId.Magic2, true), 4),
            new(new(CharacterId.Magic3, true), 4),
            new(new(CharacterId.Magic4, true), 4),
            new(new(CharacterId.Magic5, true), 4),
            new(new(CharacterId.Magic6, true), 4),
            new(new(CharacterId.Magic7, true), 4),

            CreateStuff(CharacterId.Booster1),

            CreateStuff(CharacterId.Specialty1),
            CreateStuff(CharacterId.Specialty2),
            CreateStuff(CharacterId.Specialty3),
            CreateStuff(CharacterId.Specialty4),
            CreateStuff(CharacterId.Specialty5),
            CreateStuff(CharacterId.Specialty6),
            CreateStuff(CharacterId.Specialty7),
            CreateStuff(CharacterId.Specialty8),
            CreateStuff(CharacterId.Specialty9),
            CreateStuff(CharacterId.Specialty10),
            CreateStuff(CharacterId.Specialty11),
            CreateStuff(CharacterId.Specialty12),
            CreateStuff(CharacterId.Specialty13),
            CreateStuff(CharacterId.Specialty14),
            CreateStuff(CharacterId.Specialty15),
            CreateStuff(CharacterId.Specialty16),
            CreateStuff(CharacterId.Specialty17),
            CreateStuff(CharacterId.Specialty18),
            CreateStuff(CharacterId.Specialty19),
            CreateStuff(CharacterId.Specialty20),
            CreateStuff(CharacterId.Specialty21),
            CreateStuff(CharacterId.Specialty22),
            CreateStuff(CharacterId.Specialty23),
            CreateStuff(CharacterId.Specialty24),
            CreateStuff(CharacterId.Specialty25),
            CreateStuff(CharacterId.Specialty26),
            CreateStuff(CharacterId.Specialty27),
            CreateStuff(CharacterId.Specialty28),
            CreateStuff(CharacterId.Specialty29),
            CreateStuff(CharacterId.Specialty30),

            CreateWarp(CharacterId.WarpOut1),
            CreateWarp(CharacterId.WarpOut2),
            CreateWarp(CharacterId.WarpIn1),
            CreateWarp(CharacterId.WarpIn2),

            CreateLike(CharacterId.Like0),
            CreateLike(CharacterId.Like1),
            CreateLike(CharacterId.Like2),
            CreateLike(CharacterId.Like3),
            CreateLike(CharacterId.Like4),
            CreateLike(CharacterId.Like5),
            CreateLike(CharacterId.Like6),
            CreateLike(CharacterId.Like7),
            CreateLike(CharacterId.Like8),
        };

        private static CharacterAliveInfo CreateStuff(CharacterId characterId)
            => new(new(characterId, true), 1);

        private static CharacterAliveInfo CreateWarp(CharacterId characterId)
            => new(new(characterId, true), 2);

        private static CharacterAliveInfo CreateLike(CharacterId characterId)
            => new(new(characterId, true), 3);
    }
}
