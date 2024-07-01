using System.Collections.Generic;
using System.Linq;
using Some1.Play.Info;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerCharacterSkillFront : IPlayerCharacterSkillFront
    {
        public PlayerCharacterSkillFront(CharacterSkillInfo info, CharacterSkillPropInfoGroup propInfos)
        {
            Id = info.Id;
            Props = (propInfos.GroupByCharacterSkill.GetValueOrDefault(Id)?.Values ?? Enumerable.Empty<CharacterSkillPropInfo>())
                .Select(x => new PlayerCharacterSkillPropFront(x))
                .ToDictionary(
                    x => x.Prop.Type,
                    x => (IPlayerCharacterSkillPropFront)x);
        }

        public CharacterSkillId Id { get; }

        public IReadOnlyDictionary<SkillPropType, IPlayerCharacterSkillPropFront> Props { get; }
    }
}
