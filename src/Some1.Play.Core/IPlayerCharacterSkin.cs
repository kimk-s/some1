using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IPlayerCharacterSkin
    {
        SkinId Id { get; }
        bool IsUnlocked { get; }
        bool IsElected { get; }
        bool IsSelected { get; }
        bool IsRandomSelected { get; }
    }
}
