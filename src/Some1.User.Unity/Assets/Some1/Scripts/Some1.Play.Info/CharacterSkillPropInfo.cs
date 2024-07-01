namespace Some1.Play.Info
{
    public sealed class CharacterSkillPropInfo
    {
        public CharacterSkillPropInfo(CharacterSkillId characterSkill, SkillProp prop)
        {
            CharacterSkill = characterSkill;
            Prop = prop;
        }

        public CharacterSkillId CharacterSkill { get; }

        public SkillProp Prop { get; }
    }
}
