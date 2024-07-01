using System.Collections.Generic;
using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IPlayerCharacter
    {
        CharacterId Id { get; }
        bool IsUnlocked { get; }
        bool IsPicked { get; }
        bool IsSelected { get; }
        bool IsRandomSkin { get; }
        int Exp { get; }
        int Star { get; }
        IReadOnlyDictionary<SkinId, IPlayerCharacterSkin> Skins { get; }
        IPlayerCharacterSkin ElectedSkin { get; }
    }
}
