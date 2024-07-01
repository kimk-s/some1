using System;
using System.Buffers.Binary;

namespace Some1.Play.Info
{
    public readonly struct PlayerId : IEquatable<PlayerId>
    {
        private const int BitMask = 0xffff;
        private readonly int _value;

        public PlayerId(Guid guid)
        {
            Span<byte> bytes = stackalloc byte[16];
            if (!guid.TryWriteBytes(bytes))
            {
                throw new InvalidOperationException();
            }

            _value = BinaryPrimitives.ReadInt32LittleEndian(bytes) & BitMask;
        }

        private PlayerId(int value)
        {
            _value = value & BitMask;
        }

        public static PlayerId Empty => default;

        public static PlayerId NewPlayerId() => new(Guid.NewGuid());

        public PlayerId EnsureNotEmpty()
        {
            if (this == Empty)
            {
                throw new InvalidOperationException();
            }

            return this;
        }

        public override bool Equals(object? obj) => obj is PlayerId other && Equals(other);

        public bool Equals(PlayerId other) => _value == other._value;

        public override int GetHashCode() => HashCode.Combine(_value);

        public override string ToString() => _value.ToString("X4");

        public static bool operator ==(PlayerId left, PlayerId right) => left.Equals(right);

        public static bool operator !=(PlayerId left, PlayerId right) => !(left == right);

        public static explicit operator PlayerId(int x) => new(x);

        public static implicit operator int(PlayerId x) => x._value;
    }
}
