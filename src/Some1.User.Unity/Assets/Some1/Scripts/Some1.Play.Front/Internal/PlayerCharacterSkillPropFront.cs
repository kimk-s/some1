using Some1.Play.Info;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerCharacterSkillPropFront : IPlayerCharacterSkillPropFront
    {
        public PlayerCharacterSkillPropFront(CharacterSkillPropInfo info)
        {
            Prop = info.Prop;
        }

        public SkillProp Prop { get; }
    }
}
