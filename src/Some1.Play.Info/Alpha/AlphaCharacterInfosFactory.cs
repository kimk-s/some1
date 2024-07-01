using System.Collections.Generic;

namespace Some1.Play.Info.Alpha
{
    public static class AlphaCharacterInfosFactory
    {
        public static IEnumerable<CharacterInfo> Create() => new CharacterInfo[]
        {
            new(
                CharacterId.Player1,
                CharacterType.Player,
                new(AreaType.Rectangle, PlayConst.UnitSize),
                role: CharacterRole.DamageDealer,
                move: new(BumpLevel.Low),
                shift: new(1, 1, 1, 1),
                walk: new(1, 2.57f)),
            new(
                CharacterId.Player2,
                CharacterType.Player,
                new(AreaType.Rectangle, PlayConst.UnitSize),
                role: CharacterRole.Tank,
                move: new(BumpLevel.Low),
                shift: new(1, 1, 1, 1),
                walk: new(1, 2.57f)),
            new(
                CharacterId.Player3,
                CharacterType.Player,
                new(AreaType.Rectangle, PlayConst.UnitSize),
                role: CharacterRole.DamageDealer,
                move: new(BumpLevel.Low),
                shift: new(1, 1, 1, 1),
                walk: new(1, 2.4f)),
            new(
                CharacterId.Player4,
                CharacterType.Player,
                new(AreaType.Rectangle, PlayConst.UnitSize),
                role: CharacterRole.Support,
                move: new(BumpLevel.Low),
                shift: new(1, 1, 1, 1),
                walk: new(1, 2.4f)),
            new(
                CharacterId.Player5,
                CharacterType.Player,
                new(AreaType.Rectangle, PlayConst.UnitSize),
                role: CharacterRole.Artillery,
                move: new(BumpLevel.Low),
                shift: new(1, 1, 1, 1),
                walk: new(1, 2.4f)),
            new(
                CharacterId.Player6,
                CharacterType.Player,
                new(AreaType.Rectangle, PlayConst.UnitSize),
                role: CharacterRole.Assassin,
                season: SeasonId.Season1,
                move: new(BumpLevel.Low),
                shift: new(1, 1, 1, 1),
                walk: new(1, 2.73f)),

            new(
                CharacterId.Mob1,
                CharacterType.NonPlayer,
                new(AreaType.Rectangle, PlayConst.UnitSize),
                season: SeasonId.Season1,
                move: new(BumpLevel.Low),
                shift: new(1, 1, 1, 1),
                walk: new(1, 1.5f),
                agent: new BattleCharacterAgentInfo(
                    5,
                    0,
                    0)),
            new(
                CharacterId.Mob2,
                CharacterType.NonPlayer,
                new(AreaType.Rectangle, PlayConst.UnitSize),
                season: SeasonId.Season1,
                move: new(BumpLevel.Low),
                shift: new(1, 1, 1, 1),
                walk: new(1, 1.3f),
                agent: new BattleCharacterAgentInfo(
                    5,
                    0,
                    0)),
            new(
                CharacterId.Mob3,
                CharacterType.NonPlayer,
                new(AreaType.Rectangle, PlayConst.UnitSize),
                season: SeasonId.Season1,
                move: new(BumpLevel.Low),
                shift: new(1, 1, 1, 1),
                walk: new(1, 1.2f),
                agent: new BattleCharacterAgentInfo(
                    5,
                    0,
                    0)),
            new(
                CharacterId.Mob4,
                CharacterType.NonPlayer,
                new(AreaType.Rectangle, PlayConst.UnitSize),
                season: SeasonId.Season1,
                move: new(BumpLevel.Low),
                shift: new(1, 1, 1, 1),
                walk: new(1, 1.6f),
                agent: new BattleCharacterAgentInfo(
                    5,
                    0,
                    0)),

            new(
                CharacterId.Chief1,
                CharacterType.NonPlayer,
                new(AreaType.Rectangle, PlayConst.UnitSize),
                season: SeasonId.Season1,
                move: new(BumpLevel.Low),
                shift: new(1, 1, 1, 1),
                walk: new(1, 1.6f),
                agent: new BattleCharacterAgentInfo(
                    5,
                    0.2f,
                    0)),
            new(
                CharacterId.Chief2,
                CharacterType.NonPlayer,
                new(AreaType.Rectangle, PlayConst.UnitSize),
                season: SeasonId.Season1,
                move: new(BumpLevel.Low),
                shift: new(1, 1, 1, 1),
                walk: new(1, 1.5f),
                agent: new BattleCharacterAgentInfo(
                    5,
                    0.2f,
                    0)),

            new(
                CharacterId.Boss1,
                CharacterType.NonPlayer,
                new(AreaType.Rectangle, PlayConst.UnitSize),
                season: SeasonId.Season1,
                move: new(BumpLevel.Low),
                shift: new(1, 1, 1, 1),
                walk: new(1, 1.5f),
                agent: new BattleCharacterAgentInfo(
                    5,
                    0.2f,
                    0.1f)),
            new(
                CharacterId.Boss2,
                CharacterType.NonPlayer,
                new(AreaType.Rectangle, PlayConst.UnitSize),
                season: SeasonId.Season1,
                move: new(BumpLevel.Low),
                shift: new(1, 1, 1, 1),
                walk: new(1, 1.5f),
                agent: new BattleCharacterAgentInfo(
                    5,
                    0.2f,
                    0.1f)),
            new(
                CharacterId.Boss3,
                CharacterType.NonPlayer,
                new(AreaType.Rectangle, PlayConst.UnitSize),
                season: SeasonId.Season1,
                move: new(BumpLevel.Low),
                shift: new(1, 1, 1, 1),
                walk: new(1, 1.5f),
                agent: new BattleCharacterAgentInfo(
                    5,
                    0.2f,
                    0.1f)),
            new(
                CharacterId.Boss4,
                CharacterType.NonPlayer,
                new(AreaType.Rectangle, PlayConst.UnitSize),
                season: SeasonId.Season1,
                move: new(BumpLevel.Low),
                shift: new(1, 1, 1, 1),
                walk: new(1, 2),
                agent: new BattleCharacterAgentInfo(
                    5,
                    0.2f,
                    0.2f)),
            new(
                CharacterId.Boss5,
                CharacterType.NonPlayer,
                new(AreaType.Rectangle, PlayConst.UnitSize),
                season: SeasonId.Season1,
                move: new(BumpLevel.Low),
                shift: new(1, 1, 1, 1),
                walk: new(1, 2),
                agent: new BattleCharacterAgentInfo(
                    5,
                    0.2f,
                    0.2f)),
            new(
                CharacterId.Boss6,
                CharacterType.NonPlayer,
                new(AreaType.Rectangle, PlayConst.UnitSize),
                season: SeasonId.Season1,
                move: new(BumpLevel.Low),
                shift: new(1, 1, 1, 1),
                walk: new(1, 2),
                agent: new BattleCharacterAgentInfo(
                    5,
                    0.2f,
                    0.2f)),

            CreatePlant(CharacterId.Plant1, SeasonId.Season1),
            CreatePlant(CharacterId.Plant2, SeasonId.Season1),
            CreatePlant(CharacterId.Plant3, SeasonId.Season1, new(HitAttribute.Fire, 100)),
            CreatePlant(CharacterId.Plant4, SeasonId.Season1),
            CreatePlant(CharacterId.Plant5, SeasonId.Season1),
            CreatePlant(CharacterId.Plant6, SeasonId.Season1),
            CreatePlant(CharacterId.Plant7, SeasonId.Season1, new(HitAttribute.Poison, 100)),
            CreatePlant(CharacterId.Plant8, SeasonId.Season1),

            CreateAnimal(CharacterId.Animal1, SeasonId.Season1),
            CreateAnimal(CharacterId.Animal2, SeasonId.Season1),
            CreateAnimal(CharacterId.Animal3, SeasonId.Season1),
            CreateAnimal(CharacterId.Animal4, SeasonId.Season1),
            CreateAnimal(CharacterId.Animal5, SeasonId.Season1),

            new(
                CharacterId.Summon1,
                CharacterType.NonPlayer,
                new(AreaType.Rectangle, PlayConst.UnitSize),
                season: SeasonId.Season1,
                move: new(BumpLevel.Low),
                shift: new(1, 1, 1, 1),
                walk: new(1, 1.8f),
                agent: new BattleCharacterAgentInfo(
                    9,
                    0,
                    0)),

            new(
                CharacterId.Box,
                CharacterType.NonPlayer,
                new(AreaType.Rectangle, PlayConst.UnitSize)),

            new(
                CharacterId.Roadworks,
                CharacterType.Static,
                new(AreaType.Circle, 1)),

            new(
                CharacterId.Fence1,
                CharacterType.Static,
                new(AreaType.Rectangle, 4),
                BumpLevel.High),

            new(
                CharacterId.Landmark1,
                CharacterType.Static,
                new(AreaType.Rectangle, 2),
                BumpLevel.Middle),

            new(
                CharacterId.Barrier1,
                CharacterType.Static,
                new(AreaType.Rectangle, 1),
                BumpLevel.Middle),

            new(
                CharacterId.Water1,
                CharacterType.Static,
                new(AreaType.Rectangle, 1),
                BumpLevel.Low),

            CreateMissile(CharacterId.Missile1),
            CreateMissile(CharacterId.Missile2),
            CreateMissile(CharacterId.Missile3),
            CreateMissile(CharacterId.Missile4),
            CreateMissile(CharacterId.Missile5, moveBumpLevel: BumpLevel.High),
            CreateMissile(CharacterId.Missile6),
            CreateMissile(CharacterId.Missile7, moveBumpLevel: BumpLevel.High),
            CreateMissile(CharacterId.Missile8),
            CreateMissile(CharacterId.Missile9),
            CreateMissile(CharacterId.Missile10),
            CreateMissile(CharacterId.Missile11),
            CreateMissile(CharacterId.Missile12),
            CreateMissile(CharacterId.Missile13),
            CreateMissile(CharacterId.Missile14),
            CreateMissile(CharacterId.Missile15),
            CreateMissile(CharacterId.Missile16),
            CreateMissile(CharacterId.Missile17),
            CreateMissile(CharacterId.Missile18),
            CreateMissile(CharacterId.Missile19),
            CreateMissile(CharacterId.Missile20),
            CreateMissile(CharacterId.Missile21),
            CreateMissile(CharacterId.Missile22),
            CreateMissile(CharacterId.Missile23, 0.7f),
            CreateMissile(CharacterId.Missile24, 0.7f),
            CreateMissile(CharacterId.Missile25),
            CreateMissile(CharacterId.Missile26),
            CreateMissile(CharacterId.Missile27),
            CreateMissile(CharacterId.Missile28),
            CreateMissile(CharacterId.Missile29),
            CreateMissile(CharacterId.Missile30, 0.8f),
            CreateMissile(CharacterId.Missile31, 0.8f),
            CreateMissile(CharacterId.Missile32),

            CreateExplosion(CharacterId.Explosion1),
            CreateExplosion(CharacterId.Explosion2),
            CreateExplosion(CharacterId.Explosion3),
            CreateExplosion(CharacterId.Explosion4),
            CreateExplosion(CharacterId.Explosion5),
            CreateExplosion(CharacterId.Explosion6),
            CreateExplosion(CharacterId.Explosion7),
            CreateExplosion(CharacterId.Explosion8, 4),
            CreateExplosion(CharacterId.Explosion9, 4),
            CreateExplosion(CharacterId.Explosion10),
            CreateExplosion(CharacterId.Explosion11),
            CreateExplosion(CharacterId.Explosion12),
            CreateExplosion(CharacterId.Explosion13),
            CreateExplosion(CharacterId.Explosion14),
            CreateExplosion(CharacterId.Explosion15),
            CreateExplosion(CharacterId.Explosion16),
            CreateExplosion(CharacterId.Explosion17),
            CreateExplosion(CharacterId.Explosion18),
            CreateExplosion(CharacterId.Explosion19),
            CreateExplosion(CharacterId.Explosion20),
            CreateExplosion(CharacterId.Explosion21),
            CreateExplosion(CharacterId.Explosion22),
            CreateExplosion(CharacterId.Explosion23),
            CreateExplosion(CharacterId.Explosion24),
            CreateExplosion(CharacterId.Explosion25),
            CreateExplosion(CharacterId.Explosion26),
            CreateExplosion(CharacterId.Explosion27),
            CreateExplosion(CharacterId.Explosion28),
            CreateExplosion(CharacterId.Explosion29),
            CreateExplosion(CharacterId.Explosion30),
            CreateExplosion(CharacterId.Explosion31),
            CreateExplosion(CharacterId.Explosion32),

            new(
                CharacterId.Magic1,
                CharacterType.Effect,
                new(AreaType.Circle, 8)),
            new(
                CharacterId.Magic2,
                CharacterType.Effect,
                new(AreaType.Circle, 6)),
            new(
                CharacterId.Magic3,
                CharacterType.Effect,
                new(AreaType.Circle, 6)),
            new(
                CharacterId.Magic4,
                CharacterType.Effect,
                new(AreaType.Circle, 6)),
            new(
                CharacterId.Magic5,
                CharacterType.Effect,
                new(AreaType.Circle, 6)),
            new(
                CharacterId.Magic6,
                CharacterType.Effect,
                new(AreaType.Circle, 6)),
            new(
                CharacterId.Magic7,
                CharacterType.Effect,
                new(AreaType.Circle, 6)),

            new(
                CharacterId.Booster1,
                CharacterType.Stuff,
                new(AreaType.Rectangle, 0.5f),
                giveStuff : new(new BoosterStuff(BoosterId.Power, 1))),

            CreateSpecialty(CharacterId.Specialty1, SpecialtyId.Mob1),
            CreateSpecialty(CharacterId.Specialty2, SpecialtyId.Mob2),
            CreateSpecialty(CharacterId.Specialty3, SpecialtyId.Mob3),
            CreateSpecialty(CharacterId.Specialty4, SpecialtyId.Mob4),
            CreateSpecialty(CharacterId.Specialty5, SpecialtyId.Chief1),
            CreateSpecialty(CharacterId.Specialty6, SpecialtyId.Chief2),
            CreateSpecialty(CharacterId.Specialty7, SpecialtyId.Boss1),
            CreateSpecialty(CharacterId.Specialty8, SpecialtyId.Boss2),
            CreateSpecialty(CharacterId.Specialty9, SpecialtyId.Boss3),
            CreateSpecialty(CharacterId.Specialty10, SpecialtyId.Boss4),
            CreateSpecialty(CharacterId.Specialty11, SpecialtyId.Boss5),
            CreateSpecialty(CharacterId.Specialty12, SpecialtyId.Boss6),
            CreateSpecialty(CharacterId.Specialty13, SpecialtyId.Plant1),
            CreateSpecialty(CharacterId.Specialty14, SpecialtyId.Plant2),
            CreateSpecialty(CharacterId.Specialty15, SpecialtyId.Plant3),
            CreateSpecialty(CharacterId.Specialty16, SpecialtyId.Plant4),
            CreateSpecialty(CharacterId.Specialty17, SpecialtyId.Plant5),
            CreateSpecialty(CharacterId.Specialty18, SpecialtyId.Plant6),
            CreateSpecialty(CharacterId.Specialty19, SpecialtyId.Plant7),
            CreateSpecialty(CharacterId.Specialty20, SpecialtyId.Plant8),
            CreateSpecialty(CharacterId.Specialty21, SpecialtyId.Animal1),
            CreateSpecialty(CharacterId.Specialty22, SpecialtyId.Animal2),
            CreateSpecialty(CharacterId.Specialty23, SpecialtyId.Animal3),
            CreateSpecialty(CharacterId.Specialty24, SpecialtyId.Animal4),
            CreateSpecialty(CharacterId.Specialty25, SpecialtyId.Animal5),
            CreateSpecialty(CharacterId.Specialty26, SpecialtyId.Animal6),
            CreateSpecialty(CharacterId.Specialty27, SpecialtyId.Animal7),
            CreateSpecialty(CharacterId.Specialty28, SpecialtyId.Animal8),
            CreateSpecialty(CharacterId.Specialty29, SpecialtyId.Animal9),
            CreateSpecialty(CharacterId.Specialty30, SpecialtyId.Animal10),

            new(
                CharacterId.WarpOut1,
                CharacterType.Effect,
                new(AreaType.Circle, 1)),
            new(
                CharacterId.WarpOut2,
                CharacterType.Effect,
                new(AreaType.Circle, 1)),
            new(
                CharacterId.WarpIn1,
                CharacterType.Effect,
                new(AreaType.Circle, 1)),
            new(
                CharacterId.WarpIn2,
                CharacterType.Effect,
                new(AreaType.Circle, 1)),

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

        private static CharacterInfo CreatePlant(CharacterId characterId, SeasonId season, HitReachedInfo? hitReached = null) => new(
            characterId,
            CharacterType.NonPlayer,
            new(AreaType.Rectangle, PlayConst.UnitSize),
            season: season,
            hitReached: hitReached);

        private static CharacterInfo CreateAnimal(CharacterId characterId, SeasonId season) => new(
            characterId,
            CharacterType.NonPlayer,
            new(AreaType.Rectangle, PlayConst.UnitSize),
            season: season,
            move: new(BumpLevel.Low),
            shift: new(1, 1, 1, 1),
            walk: new(1, 1),
            agent: new BattleCharacterAgentInfo(5, 0.05f, 0, 10));

        private static CharacterInfo CreateMissile(CharacterId characterId, float size = 0.5f, BumpLevel moveBumpLevel = BumpLevel.Middle) => new(
            characterId,
            CharacterType.Effect,
            new(AreaType.Circle, size),
            move: new(moveBumpLevel));

        private static CharacterInfo CreateExplosion(CharacterId characterId, float size = 0.5f) => new(
            characterId,
            CharacterType.Effect,
            new(AreaType.Circle, size));

        private static CharacterInfo CreateSpecialty(CharacterId characterId, SpecialtyId specialtyId) => new(
            characterId,
            CharacterType.Stuff,
            new(AreaType.Rectangle, 0.5f),
            giveStuff: new(new SpecialtyStuff(specialtyId, 1)));

        private static CharacterInfo CreateLike(CharacterId characterId) => new(
            characterId,
            CharacterType.Effect,
            new(AreaType.Circle, 6));
    }
}
