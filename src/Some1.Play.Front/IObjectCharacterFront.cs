using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IObjectCharacterFront
    {
        ReadOnlyReactiveProperty<CharacterId?> Id { get; }
        ReadOnlyReactiveProperty<CharacterInfo?> Info { get; }
    }
}
