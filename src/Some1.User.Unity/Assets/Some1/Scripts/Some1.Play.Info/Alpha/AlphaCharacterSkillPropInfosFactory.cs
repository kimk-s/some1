using System.Collections.Generic;

namespace Some1.Play.Info.Alpha
{
    public static class AlphaCharacterSkillPropInfosFactory
    {
        public static IEnumerable<CharacterSkillPropInfo> Create() => new CharacterSkillPropInfo[]
        {
            new(
                new(CharacterId.Player1, SkillId.Skill1),
                SkillProp.CreateDamage(50, 5)),
            new(
                new(CharacterId.Player1, SkillId.Skill1),
                SkillProp.CreateRange(SkillRange.Long)),
            new(
                new(CharacterId.Player1, SkillId.Skill1),
                SkillProp.CreateReload(SkillReload.Normal)),
            new(
                new(CharacterId.Player1, SkillId.Skill2),
                SkillProp.CreateDamage(35, 9)),
            new(
                new(CharacterId.Player1, SkillId.Skill2),
                SkillProp.CreateRange(SkillRange.Long)),

            new(
                new(CharacterId.Player2, SkillId.Skill1),
                SkillProp.CreateDamage(45, 4)),
            new(
                new(CharacterId.Player2, SkillId.Skill1),
                SkillProp.CreateRange(SkillRange.Short)),
            new(
                new(CharacterId.Player2, SkillId.Skill1),
                SkillProp.CreateReload(SkillReload.VeryFast)),
            new(
                new(CharacterId.Player2, SkillId.Skill2),
                SkillProp.CreateDefense(7)),
            new(
                new(CharacterId.Player2, SkillId.Skill2),
                SkillProp.CreateDuration(6)),

            new(
                new(CharacterId.Player3, SkillId.Skill1),
                SkillProp.CreateDamage(60, 6)),
            new(
                new(CharacterId.Player3, SkillId.Skill1),
                SkillProp.CreateRange(SkillRange.VeryLong)),
            new(
                new(CharacterId.Player3, SkillId.Skill1),
                SkillProp.CreateReload(SkillReload.Fast)),
            new(
                new(CharacterId.Player3, SkillId.Skill2),
                SkillProp.CreateDamage(140, 3)),
            new(
                new(CharacterId.Player3, SkillId.Skill2),
                SkillProp.CreateRange(SkillRange.VeryLong)),

            new(
                new(CharacterId.Player4, SkillId.Skill1),
                SkillProp.CreateDamage(85, 1)),
            new(
                new(CharacterId.Player4, SkillId.Skill1),
                SkillProp.CreateRange(SkillRange.Long)),
            new(
                new(CharacterId.Player4, SkillId.Skill1),
                SkillProp.CreateReload(SkillReload.Normal)),
            new(
                new(CharacterId.Player4, SkillId.Skill2),
                SkillProp.CreateRecovery(230)),
            new(
                new(CharacterId.Player4, SkillId.Skill2),
                SkillProp.CreateRange(SkillRange.Normal)),

            new(
                new(CharacterId.Player5, SkillId.Skill1),
                SkillProp.CreateDamage(85, 2)),
            new(
                new(CharacterId.Player5, SkillId.Skill1),
                SkillProp.CreateRange(SkillRange.Long)),
            new(
                new(CharacterId.Player5, SkillId.Skill1),
                SkillProp.CreateReload(SkillReload.Slow)),
            new(
                new(CharacterId.Player5, SkillId.Skill2),
                SkillProp.CreateDamage(85, 4)),
            new(
                new(CharacterId.Player5, SkillId.Skill2),
                SkillProp.CreateRange(SkillRange.Long)),

            new(
                new(CharacterId.Player6, SkillId.Skill1),
                SkillProp.CreateDamage(55, 3)),
            new(
                new(CharacterId.Player6, SkillId.Skill1),
                SkillProp.CreateRange(SkillRange.Long)),
            new(
                new(CharacterId.Player6, SkillId.Skill1),
                SkillProp.CreateReload(SkillReload.Fast)),
            new(
                new(CharacterId.Player6, SkillId.Skill2),
                SkillProp.CreateDamage(55, 16)),
            new(
                new(CharacterId.Player6, SkillId.Skill2),
                SkillProp.CreateRange(SkillRange.Long)),
        };
    }
}
