using System;

namespace Some1.Play.Info
{
    public readonly struct SkillProp : IEquatable<SkillProp>
    {
        public SkillProp(SkillPropType type, int value1, int value2 = 0)
        {
            Type = type;
            Value1 = value1;
            Value2 = value2;
        }

        public SkillPropType Type { get; }

        public int Value1 { get; }

        public int Value2 { get; }

        public override bool Equals(object? obj) => obj is SkillProp other && Equals(other);

        public bool Equals(SkillProp other) => Type == other.Type && Value1 == other.Value1 && Value2 == other.Value2;

        public override int GetHashCode() => HashCode.Combine(Type, Value1, Value2);

        public override string ToString() => $"<{Type} {Value1} {Value2}>";

        public static bool operator ==(SkillProp left, SkillProp right) => left.Equals(right);

        public static bool operator !=(SkillProp left, SkillProp right) => !(left == right);

        public static DamageSkillProp CreateDamage(int damage, int count) => new(damage, count);

        public static RangeSkillProp CreateRange(SkillRange value) => new(value);

        public static ReloadSkillProp CreateReload(SkillReload value) => new(value);

        public static DefenseSkillProp CreateDefense(int value) => new(value);

        public static DurationSkillProp CreateDuration(int value) => new(value);

        public static RecoverySkillProp CreateRecovery(int value) => new(value);
    }

    public enum SkillPropType
    {
        Damage,
        Range,
        Reload,
        Defense,
        Duration,
        Recovery,
    }

    public readonly struct DamageSkillProp
    {
        public DamageSkillProp(int damage, int count)
        {
            Damage = damage;
            Count = count;
        }

        public int Damage { get; }

        public int Count { get; }

        public static implicit operator SkillProp(DamageSkillProp x) => new(SkillPropType.Damage, x.Damage, x.Count);

        public static explicit operator DamageSkillProp(SkillProp x)
        {
            if (x.Type != SkillPropType.Damage)
            {
                throw new InvalidCastException();
            }

            return new(x.Value1, x.Value2);
        }
    }

    public readonly struct RangeSkillProp
    {
        public RangeSkillProp(SkillRange value)
        {
            Value = value;
        }

        public SkillRange Value { get; }

        public static implicit operator SkillProp(RangeSkillProp x) => new(SkillPropType.Range, (int)x.Value);

        public static explicit operator RangeSkillProp(SkillProp x)
        {
            if (x.Type != SkillPropType.Range)
            {
                throw new InvalidCastException();
            }

            return new((SkillRange)x.Value1);
        }
    }

    public readonly struct ReloadSkillProp
    {
        public ReloadSkillProp(SkillReload value)
        {
            Value = value;
        }

        public SkillReload Value { get; }

        public static implicit operator SkillProp(ReloadSkillProp x) => new(SkillPropType.Reload, (int)x.Value);

        public static explicit operator ReloadSkillProp(SkillProp x)
        {
            if (x.Type != SkillPropType.Reload)
            {
                throw new InvalidCastException();
            }

            return new((SkillReload)x.Value1);
        }
    }

    public readonly struct DefenseSkillProp
    {
        public DefenseSkillProp(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public static implicit operator SkillProp(DefenseSkillProp x) => new(SkillPropType.Defense, x.Value);

        public static explicit operator DefenseSkillProp(SkillProp x)
        {
            if (x.Type != SkillPropType.Defense)
            {
                throw new InvalidCastException();
            }

            return new(x.Value1);
        }
    }

    public readonly struct DurationSkillProp
    {
        public DurationSkillProp(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public static implicit operator SkillProp(DurationSkillProp x) => new(SkillPropType.Duration, x.Value);

        public static explicit operator DurationSkillProp(SkillProp x)
        {
            if (x.Type != SkillPropType.Duration)
            {
                throw new InvalidCastException();
            }

            return new(x.Value1);
        }
    }

    public readonly struct RecoverySkillProp
    {
        public RecoverySkillProp(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public static implicit operator SkillProp(RecoverySkillProp x) => new(SkillPropType.Recovery, x.Value);

        public static explicit operator RecoverySkillProp(SkillProp x)
        {
            if (x.Type != SkillPropType.Recovery)
            {
                throw new InvalidCastException();
            }

            return new(x.Value1);
        }
    }
}
