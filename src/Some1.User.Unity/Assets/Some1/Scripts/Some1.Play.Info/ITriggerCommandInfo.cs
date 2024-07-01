using System;

namespace Some1.Play.Info
{
    public interface ITriggerCommandInfo
    {
    }

    public sealed class SetShiftCommandInfo : ITriggerCommandInfo
    {
        public SetShiftCommandInfo(
            ShiftId id,
            Mixable<float> rotation,
            Mixable<float>? distance,
            float? time,
            float? speed)
        {
            int nullCount = (distance is null ? 1 : 0) + (time is null ? 1 : 0) + (speed is null ? 1 : 0);
            if (nullCount != 1)
            {
                throw new ArgumentException("Null count of (distance, time, speed) is not 1.");
            }

            if (distance is not null && distance.Value.Value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(distance));
            }

            if (time is not null && time.Value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(time));
            }

            if (speed is not null && speed.Value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(speed));
            }

            Id = id;
            Rotation = rotation;
            Distance = distance;
            Time = time;
            Speed = speed;
        }

        public ShiftId Id { get; }

        public Mixable<float> Rotation { get; }

        public Mixable<float>? Distance { get; }

        public float? Time { get; }

        public float? Speed { get; }
    }

    public sealed class AddBuffCommandInfo : ITriggerCommandInfo
    {
        public AddBuffCommandInfo(BuffId id)
        {
            Id = id;
        }

        public BuffId Id { get; }
    }

    public sealed class AddBoosterCommandInfo : ITriggerCommandInfo
    {
        public AddBoosterCommandInfo(BoosterId id)
        {
            Id = id;
        }

        public BoosterId Id { get; }
    }

    public sealed class AddSpecialtyCommandInfo : ITriggerCommandInfo
    {
        public AddSpecialtyCommandInfo(SpecialtyId id)
        {
            Id = id;
        }

        public SpecialtyId Id { get; }
    }

    public sealed class AddHitCommandInfo : ITriggerCommandInfo
    {
        public AddHitCommandInfo(HitId id, int baseValue, int traitValue)
        {
            Id = id;
            BaseValue = baseValue;
            TraitValue = traitValue;
        }

        public HitId Id { get; }

        public int BaseValue { get; }

        public int TraitValue { get; }
    }

    public sealed class AddEnergyCommandInfo : ITriggerCommandInfo
    {
        public AddEnergyCommandInfo(EnergyId id, int value)
        {
            Id = id;
            Value = value;
        }

        public EnergyId Id { get; }

        public int Value { get; }
    }

    public sealed class AddChildCommandInfo : ITriggerCommandInfo
    {
        public AddChildCommandInfo(
            CharacterId characterId,
            TargetAimInfo position,
            TargetAimInfo aim,
            float randomPositionRadius = 0)
        {
            if (randomPositionRadius < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(randomPositionRadius));
            }

            CharacterId = characterId;
            Position = position;
            Aim = aim;
            RandomPositionRadius = randomPositionRadius;
        }

        public CharacterId CharacterId { get; }

        public TargetAimInfo Position { get; }

        public TargetAimInfo Aim { get; }

        public float RandomPositionRadius { get; }
    }

    public sealed class AddMoveCommandInfo : ITriggerCommandInfo
    {
        public AddMoveCommandInfo(float rotation, float speed)
        {
            if (speed <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(speed));
            }

            Rotation = rotation;
            Speed = speed;
        }

        public float Rotation { get; }

        public float Speed { get; }
    }

    public sealed class SetCharacterCommandInfo : ITriggerCommandInfo
    {
        public SetCharacterCommandInfo(CharacterId characterId)
        {
            CharacterId = characterId;
        }

        public CharacterId CharacterId { get; }
    }
}
