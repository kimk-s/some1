using Some1.Play.Info;
using R3;

namespace Some1.Play.Core
{
    public interface IObjectCharacter
    {
        ReadOnlyReactiveProperty<CharacterId?> Id { get; }
        CharacterInfo? Info { get; }
    }
}
