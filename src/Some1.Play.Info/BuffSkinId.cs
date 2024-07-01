using System;

namespace Some1.Play.Info
{
    public readonly struct BuffSkinId : IEquatable<BuffSkinId>
    {
        public BuffSkinId(BuffId buff, SkinId skin) => (Buff, Skin) = (buff, skin);

        public BuffId Buff { get; }

        public SkinId Skin { get; }

        public override bool Equals(object? obj) => obj is BuffSkinId other && Equals(other);

        public bool Equals(BuffSkinId other) => Buff == other.Buff && Skin == other.Skin;

        public override int GetHashCode() => HashCode.Combine(Buff, Skin);

        public override string ToString() => $"<{Buff} {Skin}>";

        public static bool operator ==(BuffSkinId left, BuffSkinId right) => left.Equals(right);

        public static bool operator !=(BuffSkinId left, BuffSkinId right) => !(left == right);
    }
}
