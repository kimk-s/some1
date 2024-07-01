namespace Some1.Play.Info
{
    public enum EmojiId : byte
    {
        Joy,
        HeartEyes,
        Thumbsup,
        Sob,
        Thinking,
        Fire,
        Discord,
    }

    public static class EmojoIdExtensions
    {
        public static bool IsLike(this EmojiId id)
        {
            return id switch
            {
                EmojiId.Thumbsup => true,
                _ => false
            };
        }
    }
}
