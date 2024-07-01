using System;

namespace Some1.Play.Info
{
    public readonly struct CharacterSkinId : IEquatable<CharacterSkinId>
    {
        public CharacterSkinId(CharacterId character, SkinId skin) => (Character, Skin) = (character, skin);

        public CharacterId Character { get; }

        public SkinId Skin { get; }

        public override bool Equals(object? obj) => obj is CharacterSkinId other && Equals(other);

        public bool Equals(CharacterSkinId other) => Character == other.Character && Skin == other.Skin;

        public override int GetHashCode() => HashCode.Combine(Character, Skin);

        public override string ToString() => $"<{Character} {Skin}>";

        public static bool operator ==(CharacterSkinId left, CharacterSkinId right) => left.Equals(right);

        public static bool operator !=(CharacterSkinId left, CharacterSkinId right) => !(left == right);
    }
}
