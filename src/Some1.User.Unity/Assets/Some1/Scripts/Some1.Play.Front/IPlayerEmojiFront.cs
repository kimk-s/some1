using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IPlayerEmojiFront
    {
        EmojiId Id { get; }
        ReadOnlyReactiveProperty<CharacterSkinEmojiId?> Emoji { get; }
    }
}
