using System;
using System.Collections.Generic;

namespace Some1.Play.Core.Internal
{
    internal readonly struct BlockItem<TValue, TStatic> : IEquatable<BlockItem<TValue, TStatic>>
        where TValue : IBlockItemValue<TStatic>
        where TStatic : IBlockItemStatic
    {
        public BlockItem(TValue value)
        {
            Static = value.Static;
            Value = value;
        }

        public TStatic Static { get; }

        public TValue Value { get; }

        public override bool Equals(object? obj) => obj is BlockItem<TValue, TStatic> other && Equals(other);

        public bool Equals(BlockItem<TValue, TStatic> other) => EqualityComparer<TStatic>.Default.Equals(Static, other.Static) && EqualityComparer<TValue>.Default.Equals(Value, other.Value);

        public override int GetHashCode() => HashCode.Combine(Static, Value);

        public static bool operator ==(BlockItem<TValue, TStatic> left, BlockItem<TValue, TStatic> right) => left.Equals(right);

        public static bool operator !=(BlockItem<TValue, TStatic> left, BlockItem<TValue, TStatic> right) => !(left == right);
    }
}
