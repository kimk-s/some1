using System;

namespace Some1.Play.Info
{
    public readonly struct CharacterSkinEmojiId : IEquatable<CharacterSkinEmojiId>
    {
        public CharacterSkinEmojiId(CharacterId? character, SkinId? skin, EmojiId emoji) => (Character, Skin, Emoji) = (character, skin, emoji);

        public CharacterId? Character { get; }

        public SkinId? Skin { get; }

        public EmojiId Emoji { get; }

        public override bool Equals(object? obj) => obj is CharacterSkinEmojiId other && Equals(other);

        public bool Equals(CharacterSkinEmojiId other) => Character == other.Character && Skin == other.Skin && Emoji == other.Emoji;

        public override int GetHashCode() => HashCode.Combine(Character, Skin, Emoji);

        public override string ToString() => $"<{Character} {Skin} {Emoji}>";

        public static bool operator ==(CharacterSkinEmojiId left, CharacterSkinEmojiId right) => left.Equals(right);

        public static bool operator !=(CharacterSkinEmojiId left, CharacterSkinEmojiId right) => !(left == right);
    }
}
