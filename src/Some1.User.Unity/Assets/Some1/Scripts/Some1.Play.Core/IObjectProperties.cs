using System.Numerics;
using Some1.Play.Info;
using R3;

namespace Some1.Play.Core
{
    public interface IObjectProperties
    {
        Area Area { get; }
        CharacterType? CharacterType { get; }
        ObjectPlayer? Player { get; }
        int Power { get; }
        int RootId { get; }
        Aim Aim { get; }
        SkinId? SkinId { get; }
        ReadOnlyReactiveProperty<byte> Team { get; }
        Vector2? Anchor { get; }
        BirthId? BirthId { get; }
    }
}
