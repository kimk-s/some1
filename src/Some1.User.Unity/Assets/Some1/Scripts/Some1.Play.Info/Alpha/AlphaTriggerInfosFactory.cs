using System;
using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info.Alpha
{
    public static class AlphaTriggerInfosFactory
    {
        public static IEnumerable<TriggerInfo> Create()
        {
            var infos = new List<TriggerInfo>();

            infos.AddPlayer1();
            infos.AddPlayer2();
            infos.AddPlayer3();
            infos.AddPlayer4();
            infos.AddPlayer5();
            infos.AddPlayer6();

            infos.AddMob1();
            infos.AddMob2();
            infos.AddMob3();
            infos.AddMob4();

            infos.AddChief1();
            infos.AddChief2();

            infos.AddBoss1();
            infos.AddBoss2();
            infos.AddBoss3();
            infos.AddBoss4();
            infos.AddBoss5();
            infos.AddBoss6();

            infos.AddPlant1();
            infos.AddPlant2();
            infos.AddPlant3();
            infos.AddPlant4();
            infos.AddPlant5();
            infos.AddPlant6();
            infos.AddPlant7();
            infos.AddPlant8();

            infos.AddAnimal1();
            infos.AddAnimal2();
            infos.AddAnimal3();
            infos.AddAnimal4();
            infos.AddAnimal5();

            infos.AddSummon1();

            infos.AddBox();

            infos.AddMissile1();
            infos.AddMissile2();
            infos.AddMissile3();
            infos.AddMissile4();
            infos.AddMissile5();
            infos.AddMissile6();
            infos.AddMissile7();
            infos.AddMissile8();
            infos.AddMissile9();
            infos.AddMissile10();
            infos.AddMissile11();
            infos.AddMissile12();
            infos.AddMissile13();
            infos.AddMissile14();
            infos.AddMissile15();
            infos.AddMissile16();
            infos.AddMissile17();
            infos.AddMissile18();
            infos.AddMissile19();
            infos.AddMissile20();
            infos.AddMissile21();
            infos.AddMissile22();
            infos.AddMissile23();
            infos.AddMissile24();
            infos.AddMissile25();
            infos.AddMissile26();
            infos.AddMissile27();
            infos.AddMissile28();
            infos.AddMissile29();
            infos.AddMissile30();
            infos.AddMissile31();
            infos.AddMissile32();

            infos.AddExplosion1();
            infos.AddExplosion2();
            infos.AddExplosion3();
            infos.AddExplosion4();
            infos.AddExplosion5();
            infos.AddExplosion6();
            infos.AddExplosion7();
            infos.AddExplosion8();
            infos.AddExplosion9();
            infos.AddExplosion10();
            infos.AddExplosion11();
            infos.AddExplosion12();
            infos.AddExplosion13();
            infos.AddExplosion14();
            infos.AddExplosion15();
            infos.AddExplosion16();
            infos.AddExplosion17();
            infos.AddExplosion18();
            infos.AddExplosion19();
            infos.AddExplosion20();
            infos.AddExplosion21();
            infos.AddExplosion22();
            infos.AddExplosion23();
            infos.AddExplosion24();
            infos.AddExplosion25();
            infos.AddExplosion26();
            infos.AddExplosion27();
            infos.AddExplosion28();
            infos.AddExplosion29();
            infos.AddExplosion30();
            infos.AddExplosion31();
            infos.AddExplosion32();

            infos.AddMagic1();
            infos.AddMagic2();
            infos.AddMagic3();
            infos.AddMagic4();
            infos.AddMagic5();
            infos.AddMagic6();
            infos.AddMagic7();

            infos.AddBooster1();

            infos.AddSpecialty1();
            infos.AddSpecialty2();
            infos.AddSpecialty3();
            infos.AddSpecialty4();
            infos.AddSpecialty5();
            infos.AddSpecialty6();
            infos.AddSpecialty7();
            infos.AddSpecialty8();
            infos.AddSpecialty9();
            infos.AddSpecialty10();
            infos.AddSpecialty11();
            infos.AddSpecialty12();
            infos.AddSpecialty13();
            infos.AddSpecialty14();
            infos.AddSpecialty15();
            infos.AddSpecialty16();
            infos.AddSpecialty17();
            infos.AddSpecialty18();
            infos.AddSpecialty19();
            infos.AddSpecialty20();
            infos.AddSpecialty21();
            infos.AddSpecialty22();
            infos.AddSpecialty23();
            infos.AddSpecialty24();
            infos.AddSpecialty25();
            infos.AddSpecialty26();
            infos.AddSpecialty27();
            infos.AddSpecialty28();
            infos.AddSpecialty29();
            infos.AddSpecialty30();

            infos.AddBuff2();
            infos.AddBuff7();
            infos.AddBuff10();
            infos.AddBuff11();

            infos.AddWarpOut1();
            infos.AddWarpOut2();
            infos.AddWarpIn1();
            infos.AddWarpIn2();

            infos.AddLike0();
            infos.AddLike1();
            infos.AddLike2();
            infos.AddLike3();
            infos.AddLike4();
            infos.AddLike5();
            infos.AddLike6();
            infos.AddLike7();
            infos.AddLike8();

            return infos;
        }
            
        private static void AddPlayer1(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateAddMissilesBySector(
                new CharacterTriggerId(CharacterId.Player1, CharacterTriggerType.CastAttack),
                CharacterId.Missile1,
                Sector.RangeItems(5, 30)));

            infos.AddRange(CreateAddMissilesBySector(
                new CharacterTriggerId(CharacterId.Player1, CharacterTriggerType.CastSuper),
                CharacterId.Missile2,
                Sector.RangeItems(5, 50)));
            infos.AddRange(CreateAddMissilesBySector(
                new CharacterTriggerId(CharacterId.Player1, CharacterTriggerType.CastSuper),
                CharacterId.Missile2,
                Sector.RangeItems(4, 40, 0.5f)));
        }

        private static void AddPlayer2(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAttackMissile(0, 4));
            infos.Add(CreateAttackMissile(1, 4));
            infos.Add(CreateAttackMissile(2, 4));
            infos.Add(CreateAttackMissile(3, 4));

            static TriggerInfo CreateAttackMissile(int index, int count) => CreateAddMissileByStraight(
                new CharacterTriggerId(CharacterId.Player2, CharacterTriggerType.CastAttack),
                CharacterId.Missile3,
                positionRotationValue: (index % 2 == 0) ? 10 : -10,
                positionLength: PlayConst.UnitShootTriggerPosition,
                startCycle: (float)index / count);

            infos.Add(new(
                new CharacterTriggerId(CharacterId.Player2, CharacterTriggerType.CastSuper),
                new CycleTriggerEventInfo(new(0, CycleRepeat.NoRepeatInterval)),
                TriggerConditionInfo.All,
                new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
                ObjectTargetInfo.All,
                new(TriggerDestinationUniqueId.Transient),
                new AddBuffCommandInfo(BuffId.Buff1)));
        }

        private static void AddPlayer3(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAttackMissile(0, 6));
            infos.Add(CreateAttackMissile(1, 6));
            infos.Add(CreateAttackMissile(2, 6));
            infos.Add(CreateAttackMissile(3, 6));
            infos.Add(CreateAttackMissile(4, 6));
            infos.Add(CreateAttackMissile(5, 6));

            infos.Add(CreateSuperMissile(0, 3));
            infos.Add(CreateSuperMissile(1, 3));
            infos.Add(CreateSuperMissile(2, 3));

            static TriggerInfo CreateAttackMissile(int index, int count) => CreateAddMissileByStraight(
                new CharacterTriggerId(CharacterId.Player3, CharacterTriggerType.CastAttack),
                CharacterId.Missile4,
                positionRotationValue: (index % 2 == 0) ? 20 : -20,
                positionLength: PlayConst.UnitShootTriggerPosition * 1.2f,
                startCycle: (float)index / count);

            static TriggerInfo CreateSuperMissile(int index, int count) => CreateAddMissileByStraight(
                new CharacterTriggerId(CharacterId.Player3, CharacterTriggerType.CastSuper),
                CharacterId.Missile5,
                positionRotationValue: (index % 2 == 0) ? 20 : -20,
                positionLength: PlayConst.UnitShootTriggerPosition * 1.2f,
                startCycle: (float)index / count);
        }

        private static void AddPlayer4(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateAddMissilesBySector(
                new CharacterTriggerId(CharacterId.Player4, CharacterTriggerType.CastAttack),
                CharacterId.Missile6,
                Sector.RangeItems(3, 30)));

            infos.Add(CreateAddMissileByStraight(
                new CharacterTriggerId(CharacterId.Player4, CharacterTriggerType.CastSuper),
                CharacterId.Missile7,
                positionLength: 1.2f));
        }

        private static void AddPlayer5(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAddMissileByJump(
                new CharacterTriggerId(CharacterId.Player5, CharacterTriggerType.CastAttack),
                CharacterId.Missile8));

            infos.Add(CreateSuperMissile(0 / 4f, 0, -2));
            infos.Add(CreateSuperMissile(1 / 4f, 20, 1));
            infos.Add(CreateSuperMissile(2 / 4f, -20, 1));
            infos.Add(CreateSuperMissile(3 / 4f, 0, 2));

            static TriggerInfo CreateSuperMissile(float startCycle, float aimRotation, float aimLength) => CreateAddMissileByJump(
                new CharacterTriggerId(CharacterId.Player5, CharacterTriggerType.CastSuper),
                CharacterId.Missile9,
                aimRotationValue: aimRotation,
                aimLength: aimLength,
                startCycle: startCycle);
        }

        private static void AddPlayer6(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateAddMissilesBySector(
                new CharacterTriggerId(CharacterId.Player6, CharacterTriggerType.CastAttack),
                CharacterId.Missile10,
                Sector.RangeItems(3, 30)));

            infos.AddRange(CreateAddMissilesBySector(
                new CharacterTriggerId(CharacterId.Player6, CharacterTriggerType.CastSuper),
                CharacterId.Missile11,
                Sector.RangeItems(8, 360, 0)));
            infos.AddRange(CreateAddMissilesBySector(
                new CharacterTriggerId(CharacterId.Player6, CharacterTriggerType.CastSuper),
                CharacterId.Missile11,
                Sector.RangeItems(8, 360, 0.7f)));

            infos.Add(new TriggerInfo(
                new CharacterTriggerId(CharacterId.Player6, CharacterTriggerType.CastSuper),
                new CycleTriggerEventInfo(new(0, CycleRepeat.NoRepeatInterval)),
                TriggerConditionInfo.All,
                new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
                ObjectTargetInfo.All,
                new TriggerDestinationUniqueInfo(TriggerDestinationUniqueId.Transient),
                new SetShiftCommandInfo(
                    ShiftId.Dash,
                    new(true, 0),
                    new(false, 2.67f),
                    0.3f,
                    null)));
        }

        private static void AddMob1(this List<TriggerInfo> infos)
        {
            infos.Add(CreateDeadDropStuff(CharacterId.Mob1, CharacterId.Specialty1));

            infos.Add(CreateAddMissileByStraight(
                new CharacterTriggerId(CharacterId.Mob1, CharacterTriggerType.CastAttack),
                CharacterId.Missile12,
                startCycle: 0.5f));
        }

        private static void AddMob2(this List<TriggerInfo> infos)
        {
            infos.Add(CreateDeadDropStuff(CharacterId.Mob2, CharacterId.Specialty2));

            infos.Add(CreateAddMissileByStraight(
                new CharacterTriggerId(CharacterId.Mob2, CharacterTriggerType.CastAttack),
                CharacterId.Missile13,
                startCycle: 0.5f));
        }

        private static void AddMob3(this List<TriggerInfo> infos)
        {
            infos.Add(CreateDeadDropStuff(CharacterId.Mob3, CharacterId.Specialty3));

            infos.Add(CreateAddMissileByStraight(
                new CharacterTriggerId(CharacterId.Mob3, CharacterTriggerType.CastAttack),
                CharacterId.Missile14,
                startCycle: 0.5f));
        }

        private static void AddMob4(this List<TriggerInfo> infos)
        {
            infos.Add(CreateDeadDropStuff(CharacterId.Mob4, CharacterId.Specialty4));

            infos.Add(CreateAddMissileByStraight(
                new CharacterTriggerId(CharacterId.Mob4, CharacterTriggerType.CastAttack),
                CharacterId.Missile15,
                startCycle: 0.5f));
            infos.Add(CreateAddMissileByStraight(
                new CharacterTriggerId(CharacterId.Mob4, CharacterTriggerType.CastAttack),
                CharacterId.Missile15,
                startCycle: 0.99f));
        }

        private static void AddChief1(this List<TriggerInfo> infos)
        {
            infos.Add(CreateDeadDropStuff(CharacterId.Chief1, CharacterId.Specialty5));

            infos.AddRange(CreateAddMissilesBySector(
                new CharacterTriggerId(CharacterId.Chief1, CharacterTriggerType.CastAttack),
                CharacterId.Missile16,
                Sector.RangeItems(3, 65, 0.5f)));

            infos.Add(CreateAddChild(
                new CharacterTriggerId(CharacterId.Chief1, CharacterTriggerType.CastSuper),
                new(
                    CharacterId.Magic1,
                    new(),
                    new()),
                0.5f));
        }

        private static void AddChief2(this List<TriggerInfo> infos)
        {
            infos.Add(CreateDeadDropStuff(CharacterId.Chief2, CharacterId.Specialty6));
            
            infos.AddRange(CreateAddMissilesBySector(
                new CharacterTriggerId(CharacterId.Chief2, CharacterTriggerType.CastAttack),
                CharacterId.Missile17,
                Sector.RangeItems(3, 65, 0.5f)));

            infos.AddRange(CreateAddMissilesBySector(
                new CharacterTriggerId(CharacterId.Chief2, CharacterTriggerType.CastSuper),
                CharacterId.Missile18,
                Sector.RangeItems(8, 360, 0.5f)));
        }

        private static void AddBoss1(this List<TriggerInfo> infos)
        {
            infos.Add(CreateDeadDropStuff(CharacterId.Boss1, CharacterId.Specialty7));

            infos.AddRange(CreateAddMissilesBySector(
                new CharacterTriggerId(CharacterId.Boss1, CharacterTriggerType.CastAttack),
                CharacterId.Missile19,
                Sector.RangeItems(5, 100, 0.5f, cycleLength: 0.49f)));

            infos.AddRange(CreateAddMissilesBySector(
                new CharacterTriggerId(CharacterId.Boss1, CharacterTriggerType.CastSuper),
                CharacterId.Missile20,
                Sector.RangeItems(20, 721, 0.5f, cycleLength: 0.49f)));

            infos.Add(new(
                new CharacterTriggerId(CharacterId.Boss1, CharacterTriggerType.CastUltra),
                new CycleTriggerEventInfo(new(0.5f, CycleRepeat.NoRepeatInterval)),
                new(Trait.One, Trait.All, 1),
                new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
                ObjectTargetInfo.All,
                new(TriggerDestinationUniqueId.Transient),
                new AddBuffCommandInfo(BuffId.Buff4)));

            infos.Add(new(
                new CharacterTriggerId(CharacterId.Boss1, CharacterTriggerType.CastUltra),
                new CycleTriggerEventInfo(new(0.5f, CycleRepeat.NoRepeatInterval)),
                new(Trait.Two, Trait.All, 1),
                new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
                ObjectTargetInfo.All,
                new(TriggerDestinationUniqueId.Transient),
                new AddBuffCommandInfo(BuffId.Buff5)));
        }

        private static void AddBoss2(this List<TriggerInfo> infos)
        {
            infos.Add(CreateDeadDropStuff(CharacterId.Boss2, CharacterId.Specialty8));

            infos.AddRange(CreateAddMissilesBySector(
                new CharacterTriggerId(CharacterId.Boss2, CharacterTriggerType.CastAttack),
                CharacterId.Missile22,
                Sector.RangeItems(3, 100, 0.5f)));
            infos.AddRange(CreateAddMissilesBySector(
                new CharacterTriggerId(CharacterId.Boss2, CharacterTriggerType.CastAttack),
                CharacterId.Missile21,
                Sector.RangeItems(2, 50, 0.99f)));

            infos.AddRange(CreateAddMissilesBySector(
                new CharacterTriggerId(CharacterId.Boss2, CharacterTriggerType.CastSuper),
                CharacterId.Missile22,
                Sector.RangeItems(9, 360, 0.5f)));
            infos.AddRange(CreateAddMissilesBySector(
                new CharacterTriggerId(CharacterId.Boss2, CharacterTriggerType.CastSuper),
                CharacterId.Missile21,
                Sector.RangeItems(9, 360, 0.99f, 20)));

            infos.Add(new(
                new CharacterTriggerId(CharacterId.Boss2, CharacterTriggerType.CastUltra),
                new CycleTriggerEventInfo(new(0.5f, CycleRepeat.NoRepeatInterval)),
                new(Trait.One, Trait.All, 1),
                new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
                ObjectTargetInfo.All,
                new(TriggerDestinationUniqueId.Transient),
                new AddBuffCommandInfo(BuffId.Buff5)));

            infos.Add(new(
                new CharacterTriggerId(CharacterId.Boss2, CharacterTriggerType.CastUltra),
                new CycleTriggerEventInfo(new(0.5f, CycleRepeat.NoRepeatInterval)),
                new(Trait.Two, Trait.All, 1),
                new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
                ObjectTargetInfo.All,
                new(TriggerDestinationUniqueId.Transient),
                new AddBuffCommandInfo(BuffId.Buff6)));
        }

        private static void AddBoss3(this List<TriggerInfo> infos)
        {
            infos.Add(CreateDeadDropStuff(CharacterId.Boss3, CharacterId.Specialty9));

            infos.Add(CreateAddMissileByJump(
                new CharacterTriggerId(CharacterId.Boss3, CharacterTriggerType.CastAttack),
                CharacterId.Missile23,
                startCycle: 0.5f));

            infos.Add(CreateAddMissileByJump(
                new CharacterTriggerId(CharacterId.Boss3, CharacterTriggerType.CastSuper),
                CharacterId.Missile23,
                startCycle: 0.5f,
                aimRotationValue: 90 * 0));
            infos.Add(CreateAddMissileByJump(
                new CharacterTriggerId(CharacterId.Boss3, CharacterTriggerType.CastSuper),
                CharacterId.Missile23,
                startCycle: 0.5f,
                aimRotationValue: 90 * 1));
            infos.Add(CreateAddMissileByJump(
                new CharacterTriggerId(CharacterId.Boss3, CharacterTriggerType.CastSuper),
                CharacterId.Missile23,
                startCycle: 0.5f,
                aimRotationValue: 90 * 2));
            infos.Add(CreateAddMissileByJump(
                new CharacterTriggerId(CharacterId.Boss3, CharacterTriggerType.CastSuper),
                CharacterId.Missile23,
                startCycle: 0.5f,
                aimRotationValue: 90 * 3));

            infos.Add(new(
                new CharacterTriggerId(CharacterId.Boss3, CharacterTriggerType.CastUltra),
                new CycleTriggerEventInfo(new(0.5f, CycleRepeat.NoRepeatInterval)),
                new(Trait.One, Trait.All, 1),
                new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
                ObjectTargetInfo.All,
                new(TriggerDestinationUniqueId.Transient),
                new AddBuffCommandInfo(BuffId.Buff5)));

            infos.AddRange(Sector.RangeItems(4, 360, 0.5f, 0, 0.49f)
                .Select(x => CreateAddChild(
                    new CharacterTriggerId(CharacterId.Boss3, CharacterTriggerType.CastUltra),
                    new(
                        CharacterId.Summon1,
                        new(
                            new(false, x.rotationValue),
                            new(false, 1)),
                        new(
                            new(false, x.rotationValue),
                            new(false, 0))),
                    x.startCycle,
                    Trait.Two)));
        }

        private static void AddBoss4(this List<TriggerInfo> infos)
        {
            infos.Add(CreateDeadDropStuff(CharacterId.Boss4, CharacterId.Specialty10));

            infos.AddRange(CreateMissileSet(CharacterTriggerType.CastAttack, CharacterId.Missile26, 0));

            infos.AddRange(CreateMissileSet(CharacterTriggerType.CastSuper, CharacterId.Missile27, 0));
            infos.AddRange(CreateMissileSet(CharacterTriggerType.CastSuper, CharacterId.Missile27, 180));

            static IEnumerable<TriggerInfo> CreateMissileSet(CharacterTriggerType triggerType, CharacterId missileId, float angle)
            {
                return CreateAddMissilesBySector(
                    new CharacterTriggerId(CharacterId.Boss4, triggerType),
                    missileId,
                    new SectorItem[]
                    {
                        new(angle + -60, 0.5f),
                        new(angle + 60, 0.5f),
                        new(angle + -40, 0.65f),
                        new(angle + 40, 0.65f),
                        new(angle + -20, 0.8f),
                        new(angle + 20, 0.8f),
                        new(angle + 0, 0.99f),
                    });
            }

            infos.Add(new(
                new CharacterTriggerId(CharacterId.Boss4, CharacterTriggerType.CastUltra),
                new CycleTriggerEventInfo(new(0.5f, CycleRepeat.NoRepeatInterval)),
                new(Trait.One, Trait.All, 1),
                new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
                ObjectTargetInfo.All,
                new(TriggerDestinationUniqueId.Transient),
                new AddBuffCommandInfo(BuffId.Buff9)));

            infos.Add(CreateMagic(0.5f, 0));
            infos.Add(CreateMagic(0.65f, 4));
            infos.Add(CreateMagic(0.8f, 8));
            infos.Add(CreateMagic(0.99f, 12));

            static TriggerInfo CreateMagic(float startCycle, float positionLength)
            {
                return CreateAddChild(
                    new CharacterTriggerId(CharacterId.Boss4, CharacterTriggerType.CastUltra),
                    new(
                        CharacterId.Magic3,
                        new(
                            new(true, 0),
                            new(false, positionLength)),
                        new()),
                    startCycle: startCycle,
                    traitCondition: Trait.Two);
            }
        }

        private static void AddBoss5(this List<TriggerInfo> infos)
        {
            infos.Add(CreateDeadDropStuff(CharacterId.Boss5, CharacterId.Specialty11));

            infos.AddRange(CreateMissileSet(CharacterTriggerType.CastAttack, CharacterId.Missile28, 0, 0.5f));

            infos.AddRange(CreateMissileSet(CharacterTriggerType.CastSuper, CharacterId.Missile29, 0, 0.5f));
            infos.AddRange(CreateMissileSet(CharacterTriggerType.CastSuper, CharacterId.Missile29, 180, 0.99f));

            static IEnumerable<TriggerInfo> CreateMissileSet(CharacterTriggerType triggerType, CharacterId missileId, float angle, float startCycle)
            {
                yield return CreateMissile(triggerType, missileId, angle + 90, 1.1f, angle, startCycle);
                yield return CreateMissile(triggerType, missileId, angle - 90, 1.1f, angle, startCycle);
                yield return CreateMissile(triggerType, missileId, angle, PlayConst.UnitShootTriggerPosition, angle, startCycle);
            }

            static TriggerInfo CreateMissile(CharacterTriggerType triggerType, CharacterId missileId, float positionRotationValue, float positionLengthValue, float aimRotationValue, float startCycle)
            {
                return CreateAddChild(
                    new CharacterTriggerId(CharacterId.Boss5, triggerType),
                    new(
                        missileId,
                        new(
                            new(true, positionRotationValue),
                            new(false, positionLengthValue)),
                        new(
                            new(true, aimRotationValue),
                            new())),
                    startCycle);
            }

            infos.Add(new(
                new CharacterTriggerId(CharacterId.Boss5, CharacterTriggerType.CastUltra),
                new CycleTriggerEventInfo(new(0.5f, CycleRepeat.NoRepeatInterval)),
                new(Trait.One, Trait.All, 1),
                new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
                ObjectTargetInfo.All,
                new(TriggerDestinationUniqueId.Transient),
                new AddBuffCommandInfo(BuffId.Buff9)));

            infos.Add(CreateAddChild(
                new CharacterTriggerId(CharacterId.Boss5, CharacterTriggerType.CastUltra),
                new(
                    CharacterId.Magic4,
                    new(
                        new(true, 0),
                        new(true, 0)),
                    new()),
                traitCondition: Trait.Two));
        }

        private static void AddBoss6(this List<TriggerInfo> infos)
        {
            infos.Add(CreateDeadDropStuff(CharacterId.Boss6, CharacterId.Specialty12));

            infos.AddRange(Sector.RangeRotations(3, 360)
                .Select(x => CreateAddMissileByStraight(
                    new CharacterTriggerId(CharacterId.Boss6, CharacterTriggerType.CastAttack),
                    CharacterId.Missile30,
                    positionRotationValue: x,
                    positionLength: -2,
                    aimRotationValue: x,
                    startCycle: 0.5f)));

            infos.AddRange(Sector.RangeRotations(3, 360)
                .Select(x => CreateAddMissileByStraight(
                    new CharacterTriggerId(CharacterId.Boss6, CharacterTriggerType.CastSuper),
                    CharacterId.Missile31,
                    positionRotationValue: x,
                    positionLength: -2,
                    aimRotationValue: x,
                    startCycle: 0.5f)));
            infos.AddRange(Sector.RangeRotations(3, 360)
                .Select(x => CreateAddMissileByStraight(
                    new CharacterTriggerId(CharacterId.Boss6, CharacterTriggerType.CastSuper),
                    CharacterId.Missile31,
                    positionRotationValue: x + 180,
                    positionLength: -2,
                    aimRotationValue: x + 180,
                    startCycle: 0.99f)));

            infos.Add(new(
                new CharacterTriggerId(CharacterId.Boss6, CharacterTriggerType.CastUltra),
                new CycleTriggerEventInfo(new(0.5f, CycleRepeat.NoRepeatInterval)),
                new(Trait.One, Trait.All, 1),
                new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
                ObjectTargetInfo.All,
                new(TriggerDestinationUniqueId.Transient),
                new AddBuffCommandInfo(BuffId.Buff9)));

            infos.Add(CreateAddChild(
                new CharacterTriggerId(CharacterId.Boss6, CharacterTriggerType.CastUltra),
                new(
                    CharacterId.Magic6,
                    new(
                        new(true, 0),
                        new(true, 0)),
                    new()),
                traitCondition: Trait.Two));
        }

        private static void AddPlant1(this List<TriggerInfo> infos)
        {
            infos.Add(CreateDeadDropStuff(CharacterId.Plant1, CharacterId.Specialty13));
        }

        private static void AddPlant2(this List<TriggerInfo> infos)
        {
            infos.Add(CreateDeadDropStuff(CharacterId.Plant2, CharacterId.Specialty14));
        }

        private static void AddPlant3(this List<TriggerInfo> infos)
        {
            infos.Add(CreateDeadDropStuff(CharacterId.Plant3, CharacterId.Specialty15));

            infos.Add(new(
                new CharacterTriggerId(CharacterId.Plant3, CharacterTriggerType.HitReached),
                new EmptyTriggerEventInfo(),
                TriggerConditionInfo.All,
                new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
                ObjectTargetInfo.All,
                new(TriggerDestinationUniqueId.Transient),
                new AddBuffCommandInfo(BuffId.Buff10)));
        }

        private static void AddPlant4(this List<TriggerInfo> infos)
        {
            infos.Add(CreateDeadDropStuff(CharacterId.Plant4, CharacterId.Specialty16));
        }

        private static void AddPlant5(this List<TriggerInfo> infos)
        {
            infos.Add(CreateDeadDropStuff(CharacterId.Plant5, CharacterId.Specialty17));
        }

        private static void AddPlant6(this List<TriggerInfo> infos)
        {
            infos.Add(CreateDeadDropStuff(CharacterId.Plant6, CharacterId.Specialty18));
        }

        private static void AddPlant7(this List<TriggerInfo> infos)
        {
            infos.Add(CreateDeadDropStuff(CharacterId.Plant7, CharacterId.Specialty19));

            infos.Add(new(
                new CharacterTriggerId(CharacterId.Plant7, CharacterTriggerType.HitReached),
                new EmptyTriggerEventInfo(),
                TriggerConditionInfo.All,
                new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
                ObjectTargetInfo.All,
                new(TriggerDestinationUniqueId.Transient),
                new AddBuffCommandInfo(BuffId.Buff11)));
        }

        private static void AddPlant8(this List<TriggerInfo> infos)
        {
            infos.Add(CreateDeadDropStuff(CharacterId.Plant8, CharacterId.Specialty20));
        }

        private static void AddAnimal1(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateAnimal(CharacterId.Animal1, CharacterId.Specialty21, CharacterId.Specialty26));
        }

        private static void AddAnimal2(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateAnimal(CharacterId.Animal2, CharacterId.Specialty22, CharacterId.Specialty27));
        }

        private static void AddAnimal3(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateAnimal(CharacterId.Animal3, CharacterId.Specialty23, CharacterId.Specialty28));
        }

        private static void AddAnimal4(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateAnimal(CharacterId.Animal4, CharacterId.Specialty24, CharacterId.Specialty29));
        }

        private static void AddAnimal5(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateAnimal(CharacterId.Animal5, CharacterId.Specialty25, CharacterId.Specialty30));
        }

        private static IEnumerable<TriggerInfo> CreateAnimal(CharacterId characterId, CharacterId normalSpecialtyId, CharacterId rareSpecialtyId)
        {
            yield return CreateAddMissileByStraight(
                new CharacterTriggerId(characterId, CharacterTriggerType.CastAttack),
                CharacterId.Missile32);

            yield return CreateAddChild(
                new CharacterTriggerId(characterId, CharacterTriggerType.CastSuper),
                new(
                    normalSpecialtyId,
                    new(),
                    new(),
                    1),
                0.5f,
                probabilityCondition: 0.5f);
            yield return CreateAddChild(
                new CharacterTriggerId(characterId, CharacterTriggerType.CastSuper),
                new(
                    rareSpecialtyId,
                    new(),
                    new(),
                    1),
                0.5f,
                probabilityCondition: 0.05f);
        }

        private static void AddSummon1(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.Summon1, 15));

            infos.Add(CreateAddMissileByStraight(
                new CharacterTriggerId(CharacterId.Summon1, CharacterTriggerType.CastAttack),
                CharacterId.Missile25,
                startCycle: 0.5f));
        }

        private static void AddBox(this List<TriggerInfo> infos)
        {
            infos.Add(CreateDeadDropStuff(CharacterId.Box, CharacterId.Booster1));
        }

        private static void AddMissile1(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile1,
                10.33f,
                7.67f / 10.33f,
                new AddHitCommandInfo(HitId.Default, 50, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion1));
        }

        private static void AddMissile2(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile2,
                13.76f,
                7.67f / 13.76f,
                new AddHitCommandInfo(HitId.Default, 35, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion2,
                true));
        }

        private static void AddMissile3(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile3,
                10.87f,
                3 / 10.87f,
                new AddHitCommandInfo(HitId.Unique, 45, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion3,
                true,
                0.5f));
        }

        private static void AddMissile4(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile4,
                13.33f,
                9 / 13.33f,
                new AddHitCommandInfo(HitId.Default, 60, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion4));
        }

        private static void AddMissile5(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile5,
                16.3f,
                11 / 16.3f,
                new AddHitCommandInfo(HitId.Default, 140, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion5,
                true,
                0.7f));
        }

        private static void AddMissile6(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile6,
                8.33f,
                7 / 8.33f,
                new AddHitCommandInfo(HitId.Unique, 85, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion6,
                true,
                0.5f));
        }

        private static void AddMissile7(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile7,
                16.67f,
                7 / 16.67f,
                new AddHitCommandInfo(HitId.Recovery | HitId.Unique, 230, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Ally),
                CharacterId.Explosion7,
                true,
                0.5f));
        }

        private static void AddMissile8(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateJumpMissile(
                CharacterId.Missile8,
                5,
                CharacterId.Explosion8));
        }

        private static void AddMissile9(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateJumpMissile(
                CharacterId.Missile9,
                5,
                CharacterId.Explosion9));
        }

        private static void AddMissile10(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile10,
                13.33f,
                0.65f,
                new AddBuffCommandInfo(BuffId.Buff2),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion10));
        }

        private static void AddMissile11(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile11,
                13.33f,
                0.65f,
                new AddBuffCommandInfo(BuffId.Buff2),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion11));
        }

        private static void AddMissile12(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile12,
                4,
                0.75f,
                new AddHitCommandInfo(HitId.Default, 30, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion12));
        }

        private static void AddMissile13(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile13,
                4,
                1.5f,
                new AddHitCommandInfo(HitId.Default, 50, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion13));
        }

        private static void AddMissile14(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile14,
                4,
                2f,
                new AddHitCommandInfo(HitId.Default, 60, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion14));
        }

        private static void AddMissile15(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile15,
                4,
                1.25f,
                new AddHitCommandInfo(HitId.Default, 40, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion15));
        }

        private static void AddMissile16(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile16,
                6,
                0.61f,
                new AddHitCommandInfo(HitId.Default, 90, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion16));
        }

        private static void AddMissile17(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile17,
                6,
                1.28f,
                new AddHitCommandInfo(HitId.Default, 110, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion17));
        }

        private static void AddMissile18(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile18,
                3,
                3.33f,
                new AddHitCommandInfo(HitId.Default, 110, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion18));
        }

        private static void AddMissile19(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile19,
                6,
                1.5f,
                new AddHitCommandInfo(HitId.Default, 150, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion19));
        }

        private static void AddMissile20(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile20,
                3,
                3.67f,
                new AddHitCommandInfo(HitId.Default, 150, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion20));
        }

        private static void AddMissile21(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile21,
                6,
                1.5f,
                new AddHitCommandInfo(HitId.Default, 150, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion21));
        }

        private static void AddMissile22(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile22,
                3,
                3.67f,
                new AddHitCommandInfo(HitId.Default, 150, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion22));
        }

        private static void AddMissile23(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateJumpMissile(
                CharacterId.Missile23,
                3,
                CharacterId.Explosion23));

            infos.AddRange(CreateAddMissilesBySector(
                new CharacterTriggerId(CharacterId.Missile23, CharacterTriggerType.ShiftJump),
                CharacterId.Missile24,
                Sector.RangeItems(4, 360, 0.99f),
                rotationMix: false));
        }

        private static void AddMissile24(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile24,
                6,
                0.56f,
                new AddHitCommandInfo(HitId.Default, 150, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion22));
        }

        private static void AddMissile25(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile25,
                4,
                0.75f,
                new AddHitCommandInfo(HitId.Default, 20, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion25));
        }

        private static void AddMissile26(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile26,
                10.87f,
                0.64f,
                new AddHitCommandInfo(HitId.Default, 50, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion26));
        }

        private static void AddMissile27(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile27,
                13.33f,
                0.9f,
                new AddBuffCommandInfo(BuffId.Buff7),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion27,
                true));
        }

        private static void AddMissile28(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile28,
                10.87f,
                0.64f,
                new AddHitCommandInfo(HitId.Default, 50, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion28));
        }

        private static void AddMissile29(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile29,
                13.33f,
                0.9f,
                new AddHitCommandInfo(HitId.Default, 50, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion29,
                true,
                command2: new SetShiftCommandInfo(ShiftId.Knock, new(true, 0), 1, 0.5f, null)));
        }

        private static void AddMissile30(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile30,
                10.87f,
                0.83f,
                new AddHitCommandInfo(HitId.Default, 150, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion30));
        }

        private static void AddMissile31(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile31,
                13.33f,
                1.05f,
                new AddBuffCommandInfo(BuffId.Buff8),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion31,
                true));
        }

        private static void AddMissile32(this List<TriggerInfo> infos)
        {
            infos.AddRange(CreateStraightMissile(
                CharacterId.Missile32,
                4,
                0.75f,
                new AddHitCommandInfo(HitId.Default, 30, 0),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                CharacterId.Explosion32));
        }

        private static void AddExplosion1(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion1));
        }

        private static void AddExplosion2(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion2));
        }

        private static void AddExplosion3(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion3));
        }

        private static void AddExplosion4(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion4));
        }

        private static void AddExplosion5(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion5));
        }

        private static void AddExplosion6(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion6));
        }

        private static void AddExplosion7(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion7));
        }

        private static void AddExplosion8(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.Explosion8, 1.1f));

            infos.Add(new(
                new CharacterTriggerId(CharacterId.Explosion8, CharacterTriggerType.AliveTrue),
                new CycleTriggerEventInfo(new(0, 1f)),
                TriggerConditionInfo.All,
                new SpaceTriggerTargetInfo(new(AreaType.Circle, Aim.Zero, 0)),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                new(TriggerDestinationUniqueId.Transient),
                new AddHitCommandInfo(HitId.FireAttribute, 85, 0)));
        }

        private static void AddExplosion9(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.Explosion9, 3.1f));

            infos.Add(new(
                new CharacterTriggerId(CharacterId.Explosion9, CharacterTriggerType.AliveTrue),
                new CycleTriggerEventInfo(new(0, 1f)),
                TriggerConditionInfo.All,
                new SpaceTriggerTargetInfo(new(AreaType.Circle, Aim.Zero, 0)),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                new(TriggerDestinationUniqueId.Transient),
                new AddHitCommandInfo(HitId.FireAttribute, 85, 0)));
        }

        private static void AddExplosion10(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion10));
        }

        private static void AddExplosion11(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion11));
        }

        private static void AddExplosion12(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion12));
        }

        private static void AddExplosion13(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion13));
        }

        private static void AddExplosion14(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion14));
        }

        private static void AddExplosion15(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion15));
        }

        private static void AddExplosion16(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion16));
        }

        private static void AddExplosion17(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion17));
        }

        private static void AddExplosion18(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion18));
        }

        private static void AddExplosion19(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion19));
        }

        private static void AddExplosion20(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion20));
        }

        private static void AddExplosion21(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion21));
        }

        private static void AddExplosion22(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion22));
        }

        private static void AddExplosion23(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion23));
        }

        private static void AddExplosion24(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion24));
        }

        private static void AddExplosion25(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion25));
        }

        private static void AddExplosion26(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion26));
        }

        private static void AddExplosion27(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion27));
        }

        private static void AddExplosion28(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion28));
        }

        private static void AddExplosion29(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion29));
        }

        private static void AddExplosion30(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion30));
        }

        private static void AddExplosion31(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion31));
        }

        private static void AddExplosion32(this List<TriggerInfo> infos)
        {
            infos.Add(CreateExplosion(CharacterId.Explosion32));
        }

        private static void AddMagic1(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.Magic1, 1));

            infos.Add(new(
                new CharacterTriggerId(CharacterId.Magic1, CharacterTriggerType.AliveTrue),
                new CycleTriggerEventInfo(new(0.5f, 1f)),
                TriggerConditionInfo.All,
                new SpaceTriggerTargetInfo(new(AreaType.Circle, Aim.Zero, 0)),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                new(TriggerDestinationUniqueId.Transient),
                new AddBuffCommandInfo(BuffId.Buff3)));
        }

        private static void AddMagic2(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.Magic2, 1));

            infos.Add(new(
                new CharacterTriggerId(CharacterId.Magic2, CharacterTriggerType.AliveTrue),
                new CycleTriggerEventInfo(new(0.7f, CycleRepeat.NoRepeatInterval)),
                new(Trait.All, Trait.All, 2),
                new SpaceTriggerTargetInfo(new(AreaType.Circle, Aim.Zero, 0)),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                new(TriggerDestinationUniqueId.Transient),
                new AddBuffCommandInfo(BuffId.Buff7)));

            infos.AddRange(Sector.RangeRotations(4, 360)
                .Select(x => CreateAddChild(
                    new CharacterTriggerId(CharacterId.Magic2, CharacterTriggerType.AliveTrue),
                    new(
                        CharacterId.Magic3,
                        new(
                            new(false, x),
                            new(false, 5)),
                        new(
                            new(),
                            new())),
                    0.1f)));
        }

        private static void AddMagic3(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.Magic3, 1));

            infos.Add(new(
                new CharacterTriggerId(CharacterId.Magic3, CharacterTriggerType.AliveTrue),
                new CycleTriggerEventInfo(new(0.7f, CycleRepeat.NoRepeatInterval)),
                TriggerConditionInfo.All,
                new SpaceTriggerTargetInfo(new(AreaType.Circle, Aim.Zero, 0)),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                new(TriggerDestinationUniqueId.Transient),
                new AddBuffCommandInfo(BuffId.Buff7)));
        }

        private static void AddMagic4(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.Magic4, 1));

            infos.AddRange(CreateMagic4Or5Attack(CharacterId.Magic4));

            infos.AddRange(Sector.RangeRotations(4, 360, 45)
                .Select(x => CreateAddChild(
                    new CharacterTriggerId(CharacterId.Magic4, CharacterTriggerType.AliveTrue),
                    new(
                        CharacterId.Magic5,
                        new(
                            new(false, x),
                            new(false, 5)),
                        new(
                            new(),
                            new())),
                    0.1f)));
        }

        private static void AddMagic5(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.Magic5, 1));

            infos.AddRange(CreateMagic4Or5Attack(CharacterId.Magic5));
        }

        private static IEnumerable<TriggerInfo> CreateMagic4Or5Attack(CharacterId characterId)
        {
            yield return new(
                new CharacterTriggerId(characterId, CharacterTriggerType.AliveTrue),
                new CycleTriggerEventInfo(new(0.7f, CycleRepeat.NoRepeatInterval)),
                new(Trait.All, Trait.All, 2),
                new SpaceTriggerTargetInfo(new(AreaType.Circle, Aim.Zero, 0)),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                new(TriggerDestinationUniqueId.Transient),
                new AddHitCommandInfo(HitId.Default, 150, 0));

            yield return new(
                new CharacterTriggerId(characterId, CharacterTriggerType.AliveTrue),
                new CycleTriggerEventInfo(new(0.7f, CycleRepeat.NoRepeatInterval)),
                new(Trait.All, Trait.All, 2),
                new SpaceTriggerTargetInfo(new(AreaType.Circle, Aim.Zero, 0)),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                new(TriggerDestinationUniqueId.Transient),
                new SetShiftCommandInfo(ShiftId.Knock, new(true, 0), 1, 0.5f, null));
        }

        private static void AddMagic6(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.Magic6, 1));

            infos.AddRange(Sector.RangeRotations(6, 360)
                .Select(x => CreateAddChild(
                    new CharacterTriggerId(CharacterId.Magic6, CharacterTriggerType.AliveTrue),
                    new(
                        CharacterId.Magic7,
                        new(
                            new(false, x),
                            new(false, 6)),
                        new(
                            new(),
                            new())),
                    new CycleRepeat(0, CycleRepeat.NoRepeatInterval))));
        }

        private static void AddMagic7(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.Magic7, 1));

            infos.Add(new(
                new CharacterTriggerId(CharacterId.Magic7, CharacterTriggerType.AliveTrue),
                new CycleTriggerEventInfo(new(0.7f, CycleRepeat.NoRepeatInterval)),
                TriggerConditionInfo.All,
                new SpaceTriggerTargetInfo(new(AreaType.Circle, Aim.Zero, 0)),
                new(CharacterTypeTarget.Unit, TeamTargetInfo.Enemy),
                new(TriggerDestinationUniqueId.Transient),
                new AddBuffCommandInfo(BuffId.Buff8)));
        }

        private static void AddBooster1(this List<TriggerInfo> infos)
        {
            infos.AddStuff(CharacterId.Booster1);
        }

        private static void AddSpecialty1(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty1);
        private static void AddSpecialty2(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty2);
        private static void AddSpecialty3(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty3);
        private static void AddSpecialty4(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty4);
        private static void AddSpecialty5(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty5);
        private static void AddSpecialty6(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty6);
        private static void AddSpecialty7(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty7);
        private static void AddSpecialty8(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty8);
        private static void AddSpecialty9(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty9);
        private static void AddSpecialty10(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty10);
        private static void AddSpecialty11(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty11);
        private static void AddSpecialty12(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty12);
        private static void AddSpecialty13(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty13);
        private static void AddSpecialty14(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty14);
        private static void AddSpecialty15(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty15);
        private static void AddSpecialty16(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty16);
        private static void AddSpecialty17(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty17);
        private static void AddSpecialty18(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty18);
        private static void AddSpecialty19(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty19);
        private static void AddSpecialty20(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty20);
        private static void AddSpecialty21(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty21);
        private static void AddSpecialty22(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty22);
        private static void AddSpecialty23(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty23);
        private static void AddSpecialty24(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty24);
        private static void AddSpecialty25(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty25);
        private static void AddSpecialty26(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty26);
        private static void AddSpecialty27(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty27);
        private static void AddSpecialty28(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty28);
        private static void AddSpecialty29(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty29);
        private static void AddSpecialty30(this List<TriggerInfo> infos) => infos.AddStuff(CharacterId.Specialty30);

        private static void AddStuff(this List<TriggerInfo> infos, CharacterId stuffCharacterId)
        {
            infos.Add(CreateAliveDeath(stuffCharacterId, PlayConst.GiveStuffSeconds));
        }

        private static void AddBuff2(this List<TriggerInfo> infos)
        {
            infos.Add(new(
                new BuffTriggerId(BuffId.Buff2),
                new CycleTriggerEventInfo(new(0.24f, 0.25f)),
                TriggerConditionInfo.All,
                new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
                ObjectTargetInfo.All,
                new(TriggerDestinationUniqueId.Transient),
                new AddHitCommandInfo(HitId.PoisonAttribute, 30, 0)));
        }

        private static void AddBuff7(this List<TriggerInfo> infos)
        {
            infos.Add(new(
                new BuffTriggerId(BuffId.Buff7),
                new CycleTriggerEventInfo(new(0.09f, 0.1f)),
                TriggerConditionInfo.All,
                new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
                ObjectTargetInfo.All,
                new(TriggerDestinationUniqueId.Transient),
                new AddHitCommandInfo(HitId.Default, 20, 0)));
        }

        private static void AddBuff10(this List<TriggerInfo> infos)
        {
            infos.Add(new(
                new BuffTriggerId(BuffId.Buff10),
                new CycleTriggerEventInfo(new(0.5f, CycleRepeat.NoRepeatInterval)),
                TriggerConditionInfo.All,
                new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
                ObjectTargetInfo.All,
                new(TriggerDestinationUniqueId.Transient),
                new SetCharacterCommandInfo(CharacterId.Plant4)));
        }

        private static void AddBuff11(this List<TriggerInfo> infos)
        {
            infos.Add(new(
                new BuffTriggerId(BuffId.Buff11),
                new CycleTriggerEventInfo(new(0.5f, CycleRepeat.NoRepeatInterval)),
                TriggerConditionInfo.All,
                new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
                ObjectTargetInfo.All,
                new(TriggerDestinationUniqueId.Transient),
                new SetCharacterCommandInfo(CharacterId.Plant8)));
        }

        private static void AddWarpOut1(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.WarpOut1, 1));
        }

        private static void AddWarpOut2(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.WarpOut2, 1));
        }

        private static void AddWarpIn1(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.WarpIn1, 1));
        }

        private static void AddWarpIn2(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.WarpIn2, 1));
        }

        private static void AddLike0(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.Like0, 1));
        }

        private static void AddLike1(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.Like1, 1));
        }

        private static void AddLike2(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.Like2, 1));
        }

        private static void AddLike3(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.Like3, 1));
        }

        private static void AddLike4(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.Like4, 1));
        }

        private static void AddLike5(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.Like5, 1));
        }

        private static void AddLike6(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.Like6, 1));
        }

        private static void AddLike7(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.Like7, 1));
        }

        private static void AddLike8(this List<TriggerInfo> infos)
        {
            infos.Add(CreateAliveDeath(CharacterId.Like8, 1));
        }

        private static IEnumerable<TriggerInfo> CreateStraightMissile(
            CharacterId missileId,
            float moveSpeed,
            float aliveDeathTime,
            ITriggerCommandInfo command,
            ObjectTargetInfo objectTarget,
            CharacterId? explosionId = null,
            bool penetrate = false,
            float size = default,
            ITriggerCommandInfo? command2 = null)
        {
            yield return CreateAliveDeath(missileId, aliveDeathTime);

            yield return new(
                new CharacterTriggerId(missileId, CharacterTriggerType.AliveTrue),
                new CycleTriggerEventInfo(new(0, CycleRepeat.EveryFrameInterval)),
                TriggerConditionInfo.All,
                new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
                ObjectTargetInfo.All,
                new(TriggerDestinationUniqueId.Transient),
                new AddMoveCommandInfo(0, moveSpeed));

            yield return new(
                new CharacterTriggerId(missileId, CharacterTriggerType.AliveTrue),
                new CycleTriggerEventInfo(new(0, CycleRepeat.EveryFrameInterval)),
                TriggerConditionInfo.All,
                new SpaceTriggerTargetInfo(new(AreaType.Circle, Aim.Zero, size)),
                objectTarget,
                new(TriggerDestinationUniqueId.Scoped1),
                command,
                GetPostInfo(explosionId, penetrate, size * 0.5f));

            if (command2 is not null)
            {
                yield return new(
                    new CharacterTriggerId(missileId, CharacterTriggerType.AliveTrue),
                    new CycleTriggerEventInfo(new(0, CycleRepeat.EveryFrameInterval)),
                    TriggerConditionInfo.All,
                    new SpaceTriggerTargetInfo(new(AreaType.Circle, Aim.Zero, size)),
                    objectTarget,
                    new(TriggerDestinationUniqueId.Scoped2),
                    command2);
            }

            yield return new(
                new CharacterTriggerId(missileId, CharacterTriggerType.TransferMoveFailed),
                new EmptyTriggerEventInfo(),
                TriggerConditionInfo.All,
                new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
                ObjectTargetInfo.All,
                new(TriggerDestinationUniqueId.Transient),
                new AddEnergyCommandInfo(EnergyId.Health, -1));

            static TriggerPostInfo[] GetPostInfo(CharacterId? explosionId, bool penetrate, float positionLength)
            {
                var result = new TriggerPostInfo[(explosionId is not null ? 1 : 0) + (!penetrate ? 1 : 0)];

                int index = 0;

                if (explosionId is not null)
                {
                    result[index++] = new(
                        TriggerPostConditionInfo.Once,
                        new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
                        ObjectTargetInfo.All,
                        new(TriggerDestinationUniqueId.Transient),
                        new AddChildCommandInfo(
                            explosionId.Value,
                            new(
                                new(true, 0),
                                new(false, positionLength)),
                            new(
                                new(true, 0),
                                new(true, 0))));
                }

                if (!penetrate)
                {
                    result[index++] = new(
                        TriggerPostConditionInfo.PerHandled,
                        new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
                        ObjectTargetInfo.All,
                        new(TriggerDestinationUniqueId.Transient),
                        new AddEnergyCommandInfo(EnergyId.Health, -1));
                }

                return result;
            }
        }

        private static IEnumerable<TriggerInfo> CreateJumpMissile(
            CharacterId missileId,
            float moveSpeed,
            CharacterId explosionId)
        {
            yield return CreateAliveJump(missileId, moveSpeed);

            yield return new(
                new CharacterTriggerId(missileId, CharacterTriggerType.ShiftJump),
                new CycleTriggerEventInfo(new(0.99f, CycleRepeat.NoRepeatInterval)),
                TriggerConditionInfo.All,
                new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
                ObjectTargetInfo.All,
                new(TriggerDestinationUniqueId.Transient),
                new AddEnergyCommandInfo(EnergyId.Health, -1));

            yield return new TriggerInfo(
                new CharacterTriggerId(missileId, CharacterTriggerType.ShiftJump),
                new CycleTriggerEventInfo(new(0.99f, CycleRepeat.NoRepeatInterval)),
                TriggerConditionInfo.All,
                new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
                ObjectTargetInfo.All,
                new(TriggerDestinationUniqueId.Transient),
                new AddChildCommandInfo(
                    explosionId,
                    new(),
                    new()));
        }

        private static TriggerInfo CreateExplosion(CharacterId explosionId)
        {
            return CreateAliveDeath(explosionId, 1f);
        }

        //private static TriggerInfo CreateSword(CharacterId swordId)
        //{
        //    return CreateAliveDeath(swordId, 0.5f);
        //}

        //private static IEnumerable<TriggerInfo> CreateAddChildrenBySword(
        //    TriggerId triggerId,
        //    CharacterId swordId,
        //    CharacterId missileId,
        //    float missileAngle,
        //    int missileCount)
        //{
        //    if (missileAngle < 0 || missileAngle > 360)
        //    {
        //        throw new ArgumentOutOfRangeException(nameof(missileAngle));
        //    }

        //    bool rotationMix = missileAngle != 360;
        //    float positionLength = rotationMix ? PlayConst.UnitShootTriggerPosition : 0;

        //    yield return CreateAddChild(
        //        triggerId,
        //        new AddChildCommandInfo(
        //            swordId,
        //            new(
        //                new(rotationMix, 0),
        //                new(rotationMix, positionLength)),
        //            new(
        //                new(rotationMix, 0),
        //                new(rotationMix, 0))));

        //    foreach (var item in CreateAddChildrenBySector(
        //        triggerId,
        //        missileId,
        //        missileAngle,
        //        missileCount,
        //        rotationMix,
        //        positionLength))
        //    {
        //        yield return item;
        //    }
        //}

        private static class Sector
        {
            public static IEnumerable<SectorItem> RangeItems(
                int count,
                float angle,
                float startCycle = 0,
                float rotationValue = 0,
                float cycleLength = 0)
            {
                return RangeRotations(count, angle, rotationValue)
                    .Zip(RangeCycles(count, startCycle, cycleLength), (a, b) => (a, b))
                    .Select(x => new SectorItem(x.a, x.b));
            }

            public static IEnumerable<float> RangeRotations(int count, float angle, float rotationValue = 0)
            {
                if (count < 2)
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }

                if (angle < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(angle));
                }

                bool circle = (angle % 360) == 0;
                float begin = (circle ? 0 : -angle / 2) + rotationValue;
                float interval = angle / (circle ? count : count - 1);

                return Enumerable.Range(0, count).Select(x => begin + interval * x);
            }

            public static IEnumerable<float> RangeCycles(int count, float startCycle = 0, float cycleLength = 0)
            {
                if (count < 2)
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }

                if (startCycle < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(startCycle));
                }

                if (cycleLength < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(cycleLength));
                }

                float begin = startCycle;
                float interval = cycleLength / (count - 1);

                return Enumerable.Range(0, count).Select(x => begin + interval * x);
            }
        }

        private readonly struct SectorItem
        {
            public readonly float rotationValue;
            public readonly float startCycle;

            public SectorItem(float rotationValue, float startCycle)
            {
                this.rotationValue = rotationValue;
                this.startCycle = startCycle;
            }
        }

        private static IEnumerable<TriggerInfo> CreateAddMissilesBySector(
            TriggerId triggerId,
            CharacterId missileId,
            IEnumerable<SectorItem> sectorItems,
            bool rotationMix = true,
            float positionLength = PlayConst.UnitShootTriggerPosition,
            Trait traitCondition = Trait.All)
        {
            return sectorItems.Select(x => CreateAddMissileByStraight(
                triggerId,
                missileId,
                rotationMix,
                x.rotationValue,
                positionLength,
                rotationMix,
                x.rotationValue,
                x.startCycle,
                traitCondition));
        }

        private static TriggerInfo CreateAddMissileByStraight(
            TriggerId triggerId,
            CharacterId missileId,
            bool positionRotationIsMix = true,
            float positionRotationValue = 0,
            float positionLength = PlayConst.UnitShootTriggerPosition,
            bool aimRotationIsMix = true,
            float aimRotationValue = 0,
            float startCycle = 0,
            Trait traitCondition = Trait.All)
        {
            return CreateAddChild(
                triggerId,
                new AddChildCommandInfo(
                    missileId,
                    new(
                        new(positionRotationIsMix, positionRotationValue),
                        new(false, positionLength)),
                    new(
                        new(aimRotationIsMix, aimRotationValue),
                        new(false, 0))),
                startCycle,
                traitCondition);
        }

        private static TriggerInfo CreateAddMissileByJump(
            TriggerId triggerId,
            CharacterId missileId,
            bool positionRotationIsMix = true,
            float positionRotationValue = 0,
            float positionLength = 0,
            bool aimRotationIsMix = true,
            float aimRotationValue = 0,
            float aimLength = 0,
            float startCycle = 0,
            Trait traitCondition = Trait.All)
        {
            return CreateAddChild(
               triggerId,
               new AddChildCommandInfo(
                   missileId,
                   new(
                       new(positionRotationIsMix, positionRotationValue),
                       new(false, positionLength)),
                   new(
                       new(aimRotationIsMix, aimRotationValue),
                       new(true, aimLength))),
               startCycle,
               traitCondition);
        }

        private static TriggerInfo CreateAddChild(TriggerId triggerId, AddChildCommandInfo command, float startCycle = 0, Trait traitCondition = Trait.All, float probabilityCondition = 1)
            => CreateAddChild(triggerId, command, new CycleRepeat(startCycle, 1), traitCondition, probabilityCondition);

        private static TriggerInfo CreateAddChild(TriggerId triggerId, AddChildCommandInfo command, CycleRepeat cycleRepeat, Trait traitCondition = Trait.All, float probabilityCondition = 1) => new(
            triggerId,
            new CycleTriggerEventInfo(cycleRepeat),
            new(traitCondition, Trait.All, probabilityCondition),
            new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
            ObjectTargetInfo.All,
            new(TriggerDestinationUniqueId.Transient),
            command);

        private static TriggerInfo CreateAliveJump(CharacterId characterId, float speed) => new(
            new CharacterTriggerId(characterId, CharacterTriggerType.AliveTrue),
            new CycleTriggerEventInfo(new(0, CycleRepeat.NoRepeatInterval)),
            TriggerConditionInfo.All,
            new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
            ObjectTargetInfo.All,
            new(TriggerDestinationUniqueId.Transient),
            new SetShiftCommandInfo(
                ShiftId.Jump,
                new(true, 0),
                new(true, 0),
                null,
                speed));

        private static TriggerInfo CreateAliveDeath(CharacterId characterId, float time) => new(
            new CharacterTriggerId(characterId, CharacterTriggerType.AliveTrue),
            new CycleTriggerEventInfo(new(time, time)),
            TriggerConditionInfo.All,
            new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
            ObjectTargetInfo.All,
            new(TriggerDestinationUniqueId.Transient),
            new AddEnergyCommandInfo(
                EnergyId.Health,
                -100_000));

        private static TriggerInfo CreateDeadDropStuff(CharacterId characterId, CharacterId stuffId, float probabilityCondition = 0.5f) => new(
            new CharacterTriggerId(characterId, CharacterTriggerType.AliveFalse),
            new CycleTriggerEventInfo(new(0, CycleRepeat.NoRepeatInterval)),
            new(Trait.All, Trait.All, probabilityCondition),
            new HierarchyTriggerTargetInfo(HierarchyTarget.Self),
            ObjectTargetInfo.All,
            new(TriggerDestinationUniqueId.Transient),
            new AddChildCommandInfo(
                stuffId,
                new(),
                new(),
                0.5f));
    }
}
