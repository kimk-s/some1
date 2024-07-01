using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class CharacterSkillInfoGroup
    {
        public CharacterSkillInfoGroup(IEnumerable<CharacterSkillInfo> all)
        {
            ById = all.ToDictionary(x => x.Id);

            GroupByCharacterId = all.GroupBy(x => x.Id.Character)
                .ToDictionary(
                    x => x.Key,
                    x => (IReadOnlyDictionary<SkillId, CharacterSkillInfo>)x.ToDictionary(x => x.Id.Skill));
        }

        public IReadOnlyDictionary<CharacterSkillId, CharacterSkillInfo> ById { get; }

        public IReadOnlyDictionary<CharacterId, IReadOnlyDictionary<SkillId, CharacterSkillInfo>> GroupByCharacterId { get; }
    }
}
