using System;

namespace Some1.Play.Info
{
    public readonly struct TriggerId : IEquatable<TriggerId>
    {
        public TriggerId(TriggerType type, int param1 = default, int param2 = default)
        {
            Type = type;
            Param1 = param1;
            Param2 = param2;
        }

        public TriggerType Type { get; }

        public int Param1 { get; }

        public int Param2 { get; }

        public override bool Equals(object? obj) => obj is TriggerId other && Equals(other);

        public bool Equals(TriggerId other) => Type == other.Type && Param1 == other.Param1 && Param2 == other.Param2;

        public override int GetHashCode() => HashCode.Combine(Type, Param1, Param2);

        public override string ToString() => $"<{Type} {Param1} {Param2}>";

        public static bool operator ==(TriggerId left, TriggerId right) => left.Equals(right);

        public static bool operator !=(TriggerId left, TriggerId right) => !(left == right);
    }

    public readonly struct CharacterTriggerId
    {
        public CharacterTriggerId(CharacterId id, CharacterTriggerType type)
        {
            Id = id;
            Type = type;
        }

        public CharacterId Id { get; }

        public CharacterTriggerType Type { get; }

        public static implicit operator TriggerId(CharacterTriggerId character)
        {
            return new(TriggerType.Character, (int)character.Id, (int)character.Type);
        }

        public static explicit operator CharacterTriggerId(TriggerId common)
        {
            if (common.Type != TriggerType.Character)
            {
                throw new InvalidOperationException();
            }
            return new((CharacterId)common.Param1, (CharacterTriggerType)common.Param2);
        }
    }

    public enum CharacterTriggerType
    {
        AliveTrue,
        AliveFalse,
        IdleTrue,
        IdleFalse,
        ShiftJump,
        ShiftKnock,
        ShiftDash,
        ShiftGrab,
        CastAttack,
        CastSuper,
        CastUltra,
        HitReached,
        EnergyReached,
        TransferMoveFailed,
    }

    public readonly struct BuffTriggerId
    {
        public BuffTriggerId(BuffId id)
        {
            Id = id;
        }

        public BuffId Id { get; }

        public static implicit operator TriggerId(BuffTriggerId buff)
        {
            return new(TriggerType.Buff, (int)buff.Id);
        }

        public static explicit operator BuffTriggerId(TriggerId common)
        {
            if (common.Type != TriggerType.Buff)
            {
                throw new InvalidOperationException();
            }
            return new((BuffId)common.Param1);
        }
    }
}
