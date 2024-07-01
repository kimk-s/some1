using System;

namespace Some1.Play.Info
{
    public readonly struct ObjectPlayer : IEquatable<ObjectPlayer>
    {
        public ObjectPlayer(Title title)
        {
            Title = title;
        }

        public Title Title { get; }

        public override bool Equals(object? obj) => obj is ObjectPlayer other && Equals(other);

        public bool Equals(ObjectPlayer other) => Title.Equals(other.Title);

        public override int GetHashCode() => HashCode.Combine(Title);

        public override string ToString() => Title.ToString();

        public static bool operator ==(ObjectPlayer left, ObjectPlayer right) => left.Equals(right);

        public static bool operator !=(ObjectPlayer left, ObjectPlayer right) => !(left == right);
    }
}
