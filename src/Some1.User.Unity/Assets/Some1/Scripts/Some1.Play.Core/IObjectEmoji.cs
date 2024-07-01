using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IObjectEmoji
    {
        EmojiId? Emoji { get; }
        byte Level { get; }
        float Cycles { get; }
    }
}
