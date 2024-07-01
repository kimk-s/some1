using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IPlayerEmoji
    {
        EmojiId? Emoji { get; }
        float Delay { get; }
        float LikeDelay { get; }
    }
}
