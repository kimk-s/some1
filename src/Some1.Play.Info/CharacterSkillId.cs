using System;

namespace Some1.Play.Info
{
    public readonly struct CharacterSkillId : IEquatable<CharacterSkillId>
    {
        public CharacterSkillId(CharacterId character, SkillId skill)
        {
            Character = character;
            Skill = skill;
        }

        public CharacterId Character { get; }

        public SkillId Skill { get; }

        public override bool Equals(object? obj) => obj is CharacterSkillId other && Equals(other);

        public bool Equals(CharacterSkillId other) => Character == other.Character && Skill == other.Skill;

        public override int GetHashCode() => HashCode.Combine(Character, Skill);

        public override string ToString() => $"<{Character} {Skill}>";

        public static bool operator ==(CharacterSkillId left, CharacterSkillId right) => left.Equals(right);

        public static bool operator !=(CharacterSkillId left, CharacterSkillId right) => !(left == right);
    }
}
