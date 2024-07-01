using System.Collections.Generic;

namespace Some1.Play.Info.Alpha
{
    public static class AlphaCharacterSkillInfosFactory
    {
        public static IEnumerable<CharacterSkillInfo> Create() => new CharacterSkillInfo[]
        {
            new(new(CharacterId.Player1, SkillId.Skill1)),
            new(new(CharacterId.Player1, SkillId.Skill2)),

            new(new(CharacterId.Player2, SkillId.Skill1)),
            new(new(CharacterId.Player2, SkillId.Skill2)),

            new(new(CharacterId.Player3, SkillId.Skill1)),
            new(new(CharacterId.Player3, SkillId.Skill2)),

            new(new(CharacterId.Player4, SkillId.Skill1)),
            new(new(CharacterId.Player4, SkillId.Skill2)),

            new(new(CharacterId.Player5, SkillId.Skill1)),
            new(new(CharacterId.Player5, SkillId.Skill2)),

            new(new(CharacterId.Player6, SkillId.Skill1)),
            new(new(CharacterId.Player6, SkillId.Skill2)),
        };
    }
}
