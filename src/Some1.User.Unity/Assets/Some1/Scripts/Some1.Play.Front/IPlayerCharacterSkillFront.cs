using System.Collections.Generic;
using Some1.Play.Info;

namespace Some1.Play.Front
{
    public interface IPlayerCharacterSkillFront
    {
        CharacterSkillId Id { get; }
        IReadOnlyDictionary<SkillPropType, IPlayerCharacterSkillPropFront> Props { get; }
    }
}
