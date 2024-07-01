using System.Collections.Generic;

namespace Some1.Play.Info.Alpha
{
    public static class AlphaCharacterSkinEmojiInfosFactory
    {
        public static IEnumerable<CharacterSkinEmojiInfo> Create() => new CharacterSkinEmojiInfo[]
        {
            new(new(null, null, EmojiId.Joy)),
            new(new(null, null, EmojiId.HeartEyes)),
            new(new(null, null, EmojiId.Thumbsup)),
            new(new(null, null, EmojiId.Sob)),
            new(new(null, null, EmojiId.Thinking)),
            new(new(null, null, EmojiId.Fire)),
            new(new(null, null, EmojiId.Discord)),
        };
    }
}
