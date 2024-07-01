using System;

namespace Some1.Play.Info
{
    public enum StuffType : byte
    {
        Booster,
        Specialty,
    }

    public readonly struct BoosterStuff
    {
        public BoosterStuff(BoosterId id, int number)
        {
            Id = id;
            Number = number;
        }

        public BoosterId Id { get; }

        public int Number { get; }

        public override string ToString() => $"<{Id} {Number}>";

        public static implicit operator Stuff(BoosterStuff booster)
            => new(StuffType.Booster, (int)booster.Id, booster.Number);

        public static explicit operator BoosterStuff(Stuff stuff)
        {
            if (stuff.Type != StuffType.Booster)
            {
                throw new InvalidOperationException();
            }

            return new((BoosterId)stuff.Id, stuff.Number);
        }
    }

    public readonly struct SpecialtyStuff
    {
        public SpecialtyStuff(SpecialtyId id, int number)
        {
            Id = id;
            Number = number;
        }

        public SpecialtyId Id { get; }

        public int Number { get; }

        public override string ToString() => $"<{Id} {Number}>";

        public static implicit operator Stuff(SpecialtyStuff Specialty)
            => new(StuffType.Specialty, (int)Specialty.Id, Specialty.Number);

        public static explicit operator SpecialtyStuff(Stuff stuff)
        {
            if (stuff.Type != StuffType.Specialty)
            {
                throw new InvalidOperationException();
            }

            return new((SpecialtyId)stuff.Id, stuff.Number);
        }
    }

    public readonly struct Stuff : IEquatable<Stuff>
    {
        public Stuff(StuffType type, int param1, int param2)
        {
            Type = type;
            Id = param1;
            Number = param2;
        }

        public StuffType Type { get; }

        public int Id { get; }

        public int Number { get; }

        public int Score => Number * 10;

        public override bool Equals(object? obj) => obj is Stuff other && Equals(other);

        public bool Equals(Stuff other) => Type == other.Type && Id == other.Id && Number == other.Number;

        public override int GetHashCode() => HashCode.Combine(Type, Id, Number);

        public override string ToString() => $"<{Type} {Id} {Number}>";

        public static bool operator ==(Stuff left, Stuff right) => left.Equals(right);

        public static bool operator !=(Stuff left, Stuff right) => !(left == right);
    }
}
