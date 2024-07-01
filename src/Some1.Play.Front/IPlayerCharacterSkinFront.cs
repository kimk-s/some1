using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IPlayerCharacterSkinFront
    {
        CharacterSkinId Id { get; }
        ReadOnlyReactiveProperty<bool> IsUnlocked { get; }
        ReadOnlyReactiveProperty<bool> IsElected { get; }
        ReadOnlyReactiveProperty<bool> IsSelected { get; }
        ReadOnlyReactiveProperty<bool> IsRandomSelected { get; }
    }
}
