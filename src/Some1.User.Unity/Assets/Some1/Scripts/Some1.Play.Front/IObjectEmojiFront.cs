using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IObjectEmojiFront
    {
        ReadOnlyReactiveProperty<CharacterSkinEmojiId?> Emoji { get; }
        ReadOnlyReactiveProperty<byte> Level { get; }
        ReadOnlyReactiveProperty<float> Cycles { get; }
    }
}
