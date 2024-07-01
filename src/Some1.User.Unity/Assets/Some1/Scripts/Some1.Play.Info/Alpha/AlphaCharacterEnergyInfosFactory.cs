using System;
using System.Collections.Generic;

namespace Some1.Play.Info.Alpha
{
    public static class AlphaCharacterEnergyInfosFactory
    {
        public static IEnumerable<CharacterEnergyInfo> Create(AlphaPlayInfoEnvironment environment)
        {
            var e = environment;

            return new CharacterEnergyInfo[]
            {
                new(new(CharacterId.Player1, EnergyId.Health), 1_200),
                new(new(CharacterId.Player2, EnergyId.Health), 1_700),
                new(new(CharacterId.Player3, EnergyId.Health), 900),
                new(new(CharacterId.Player4, EnergyId.Health), 1_200),
                new(new(CharacterId.Player5, EnergyId.Health), 800),
                new(new(CharacterId.Player6, EnergyId.Health), 800),

                new(new(CharacterId.Mob1, EnergyId.Health), CutValue(700, e)),
                new(new(CharacterId.Mob2, EnergyId.Health), CutValue(500, e)),
                new(new(CharacterId.Mob3, EnergyId.Health), CutValue(400, e)),
                new(new(CharacterId.Mob4, EnergyId.Health), CutValue(400, e)),

                new(new(CharacterId.Chief1, EnergyId.Health), CutValue(5_500, e)),
                new(new(CharacterId.Chief2, EnergyId.Health), CutValue(4_500, e)),

                new(new(CharacterId.Boss1, EnergyId.Health), CutValue(20_000, e)),
                new(new(CharacterId.Boss2, EnergyId.Health), CutValue(20_000, e)),
                new(new(CharacterId.Boss3, EnergyId.Health), CutValue(20_000, e)),
                new(new(CharacterId.Boss4, EnergyId.Health), CutValue(10_000, e)),
                new(new(CharacterId.Boss5, EnergyId.Health), CutValue(10_000, e)),
                new(new(CharacterId.Boss6, EnergyId.Health), CutValue(10_000, e)),

                new(new(CharacterId.Plant1, EnergyId.Health), CutValue(2_000, e)),
                new(new(CharacterId.Plant2, EnergyId.Health), CutValue(2_000, e)),
                new(new(CharacterId.Plant3, EnergyId.Health), CutValue(2_000, e)),
                new(new(CharacterId.Plant4, EnergyId.Health), CutValue(2_000, e)),
                new(new(CharacterId.Plant5, EnergyId.Health), CutValue(2_000, e)),
                new(new(CharacterId.Plant6, EnergyId.Health), CutValue(2_000, e)),
                new(new(CharacterId.Plant7, EnergyId.Health), CutValue(2_000, e)),
                new(new(CharacterId.Plant8, EnergyId.Health), CutValue(2_000, e)),

                new(new(CharacterId.Animal1, EnergyId.Health), 1_000),
                new(new(CharacterId.Animal2, EnergyId.Health), 1_000),
                new(new(CharacterId.Animal3, EnergyId.Health), 1_000),
                new(new(CharacterId.Animal4, EnergyId.Health), 1_000),
                new(new(CharacterId.Animal5, EnergyId.Health), 1_000),

                new(new(CharacterId.Summon1, EnergyId.Health), CutValue(2_000, e)),

                new(new(CharacterId.Box, EnergyId.Health), 500),

                new(new(CharacterId.Roadworks, EnergyId.Health), 1),

                new(new(CharacterId.Fence1, EnergyId.Health), 1),

                new(new(CharacterId.Landmark1, EnergyId.Health), 1),

                new(new(CharacterId.Barrier1, EnergyId.Health), 1),

                new(new(CharacterId.Water1, EnergyId.Health), 1),

                new(new(CharacterId.Missile1, EnergyId.Health), 1),
                new(new(CharacterId.Missile2, EnergyId.Health), 1),
                new(new(CharacterId.Missile3, EnergyId.Health), 1),
                new(new(CharacterId.Missile4, EnergyId.Health), 1),
                new(new(CharacterId.Missile5, EnergyId.Health), 1),
                new(new(CharacterId.Missile6, EnergyId.Health), 1),
                new(new(CharacterId.Missile7, EnergyId.Health), 1),
                new(new(CharacterId.Missile8, EnergyId.Health), 1),
                new(new(CharacterId.Missile9, EnergyId.Health), 1),
                new(new(CharacterId.Missile10, EnergyId.Health), 1),
                new(new(CharacterId.Missile11, EnergyId.Health), 1),
                new(new(CharacterId.Missile12, EnergyId.Health), 1),
                new(new(CharacterId.Missile13, EnergyId.Health), 1),
                new(new(CharacterId.Missile14, EnergyId.Health), 1),
                new(new(CharacterId.Missile15, EnergyId.Health), 1),
                new(new(CharacterId.Missile16, EnergyId.Health), 1),
                new(new(CharacterId.Missile17, EnergyId.Health), 1),
                new(new(CharacterId.Missile18, EnergyId.Health), 1),
                new(new(CharacterId.Missile19, EnergyId.Health), 1),
                new(new(CharacterId.Missile20, EnergyId.Health), 1),
                new(new(CharacterId.Missile21, EnergyId.Health), 1),
                new(new(CharacterId.Missile22, EnergyId.Health), 1),
                new(new(CharacterId.Missile23, EnergyId.Health), 1),
                new(new(CharacterId.Missile24, EnergyId.Health), 1),
                new(new(CharacterId.Missile25, EnergyId.Health), 1),
                new(new(CharacterId.Missile26, EnergyId.Health), 1),
                new(new(CharacterId.Missile27, EnergyId.Health), 1),
                new(new(CharacterId.Missile28, EnergyId.Health), 1),
                new(new(CharacterId.Missile29, EnergyId.Health), 1),
                new(new(CharacterId.Missile30, EnergyId.Health), 1),
                new(new(CharacterId.Missile31, EnergyId.Health), 1),
                new(new(CharacterId.Missile32, EnergyId.Health), 1),

                new(new(CharacterId.Explosion1, EnergyId.Health), 1),
                new(new(CharacterId.Explosion2, EnergyId.Health), 1),
                new(new(CharacterId.Explosion3, EnergyId.Health), 1),
                new(new(CharacterId.Explosion4, EnergyId.Health), 1),
                new(new(CharacterId.Explosion5, EnergyId.Health), 1),
                new(new(CharacterId.Explosion6, EnergyId.Health), 1),
                new(new(CharacterId.Explosion7, EnergyId.Health), 1),
                new(new(CharacterId.Explosion8, EnergyId.Health), 1),
                new(new(CharacterId.Explosion9, EnergyId.Health), 1),
                new(new(CharacterId.Explosion10, EnergyId.Health), 1),
                new(new(CharacterId.Explosion11, EnergyId.Health), 1),
                new(new(CharacterId.Explosion12, EnergyId.Health), 1),
                new(new(CharacterId.Explosion13, EnergyId.Health), 1),
                new(new(CharacterId.Explosion14, EnergyId.Health), 1),
                new(new(CharacterId.Explosion15, EnergyId.Health), 1),
                new(new(CharacterId.Explosion16, EnergyId.Health), 1),
                new(new(CharacterId.Explosion17, EnergyId.Health), 1),
                new(new(CharacterId.Explosion18, EnergyId.Health), 1),
                new(new(CharacterId.Explosion19, EnergyId.Health), 1),
                new(new(CharacterId.Explosion20, EnergyId.Health), 1),
                new(new(CharacterId.Explosion21, EnergyId.Health), 1),
                new(new(CharacterId.Explosion22, EnergyId.Health), 1),
                new(new(CharacterId.Explosion23, EnergyId.Health), 1),
                new(new(CharacterId.Explosion24, EnergyId.Health), 1),
                new(new(CharacterId.Explosion25, EnergyId.Health), 1),
                new(new(CharacterId.Explosion26, EnergyId.Health), 1),
                new(new(CharacterId.Explosion27, EnergyId.Health), 1),
                new(new(CharacterId.Explosion28, EnergyId.Health), 1),
                new(new(CharacterId.Explosion29, EnergyId.Health), 1),
                new(new(CharacterId.Explosion30, EnergyId.Health), 1),
                new(new(CharacterId.Explosion31, EnergyId.Health), 1),
                new(new(CharacterId.Explosion32, EnergyId.Health), 1),

                new(new(CharacterId.Magic1, EnergyId.Health), 1),
                new(new(CharacterId.Magic2, EnergyId.Health), 1),
                new(new(CharacterId.Magic3, EnergyId.Health), 1),
                new(new(CharacterId.Magic4, EnergyId.Health), 1),
                new(new(CharacterId.Magic5, EnergyId.Health), 1),
                new(new(CharacterId.Magic6, EnergyId.Health), 1),
                new(new(CharacterId.Magic7, EnergyId.Health), 1),

                new(new(CharacterId.Booster1, EnergyId.Health), 1),

                new(new(CharacterId.Specialty1, EnergyId.Health), 1),
                new(new(CharacterId.Specialty2, EnergyId.Health), 1),
                new(new(CharacterId.Specialty3, EnergyId.Health), 1),
                new(new(CharacterId.Specialty4, EnergyId.Health), 1),
                new(new(CharacterId.Specialty5, EnergyId.Health), 1),
                new(new(CharacterId.Specialty6, EnergyId.Health), 1),
                new(new(CharacterId.Specialty7, EnergyId.Health), 1),
                new(new(CharacterId.Specialty8, EnergyId.Health), 1),
                new(new(CharacterId.Specialty9, EnergyId.Health), 1),
                new(new(CharacterId.Specialty10, EnergyId.Health), 1),
                new(new(CharacterId.Specialty11, EnergyId.Health), 1),
                new(new(CharacterId.Specialty12, EnergyId.Health), 1),
                new(new(CharacterId.Specialty13, EnergyId.Health), 1),
                new(new(CharacterId.Specialty14, EnergyId.Health), 1),
                new(new(CharacterId.Specialty15, EnergyId.Health), 1),
                new(new(CharacterId.Specialty16, EnergyId.Health), 1),
                new(new(CharacterId.Specialty17, EnergyId.Health), 1),
                new(new(CharacterId.Specialty18, EnergyId.Health), 1),
                new(new(CharacterId.Specialty19, EnergyId.Health), 1),
                new(new(CharacterId.Specialty20, EnergyId.Health), 1),
                new(new(CharacterId.Specialty21, EnergyId.Health), 1),
                new(new(CharacterId.Specialty22, EnergyId.Health), 1),
                new(new(CharacterId.Specialty23, EnergyId.Health), 1),
                new(new(CharacterId.Specialty24, EnergyId.Health), 1),
                new(new(CharacterId.Specialty25, EnergyId.Health), 1),
                new(new(CharacterId.Specialty26, EnergyId.Health), 1),
                new(new(CharacterId.Specialty27, EnergyId.Health), 1),
                new(new(CharacterId.Specialty28, EnergyId.Health), 1),
                new(new(CharacterId.Specialty29, EnergyId.Health), 1),
                new(new(CharacterId.Specialty30, EnergyId.Health), 1),

                new(new(CharacterId.WarpOut1, EnergyId.Health), 1),
                new(new(CharacterId.WarpOut2, EnergyId.Health), 1),
                new(new(CharacterId.WarpIn1, EnergyId.Health), 1),
                new(new(CharacterId.WarpIn2, EnergyId.Health), 1),

                new(new(CharacterId.Like0, EnergyId.Health), 1),
                new(new(CharacterId.Like1, EnergyId.Health), 1),
                new(new(CharacterId.Like2, EnergyId.Health), 1),
                new(new(CharacterId.Like3, EnergyId.Health), 1),
                new(new(CharacterId.Like4, EnergyId.Health), 1),
                new(new(CharacterId.Like5, EnergyId.Health), 1),
                new(new(CharacterId.Like6, EnergyId.Health), 1),
                new(new(CharacterId.Like7, EnergyId.Health), 1),
                new(new(CharacterId.Like8, EnergyId.Health), 1),
            };
        }

        private static int CutValue(int value, AlphaPlayInfoEnvironment environment)
        {
            if (environment.IsProduction())
            {
                return value;
            }

            if (value > 100_000)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            if (value <= 1_000)
            {
                return value;
            }

            return 1_000 + value / 100;
        }
    }
}
