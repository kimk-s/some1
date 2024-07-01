using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class CharacterSkillPropInfoGroup
    {
        public CharacterSkillPropInfoGroup(IEnumerable<CharacterSkillPropInfo> all)
        {
            GroupByCharacterSkill = all.GroupBy(x => x.CharacterSkill)
                .ToDictionary(
                    x => x.Key,
                    x => (IReadOnlyDictionary<SkillPropType, CharacterSkillPropInfo>)x.ToDictionary(x => x.Prop.Type));
        }

        public IReadOnlyDictionary<CharacterSkillId, IReadOnlyDictionary<SkillPropType, CharacterSkillPropInfo>> GroupByCharacterSkill { get; }
    }
}
