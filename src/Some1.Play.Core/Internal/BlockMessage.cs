using System;
using System.Collections.Generic;

namespace Some1.Play.Core.Internal
{
    internal readonly struct BlockMessage<T> : IEquatable<BlockMessage<T>>
    {
        public readonly bool isAdd;
        public readonly T item;

        public BlockMessage(bool isAdd, T item)
        {
            this.isAdd = isAdd;
            this.item = item;
        }

        public override bool Equals(object? obj) => obj is BlockMessage<T> other && Equals(other);

        public bool Equals(BlockMessage<T> other) => isAdd == other.isAdd && EqualityComparer<T>.Default.Equals(item, other.item);

        public override int GetHashCode() => HashCode.Combine(isAdd, item);

        public static bool operator ==(BlockMessage<T> left, BlockMessage<T> right) => left.Equals(right);

        public static bool operator !=(BlockMessage<T> left, BlockMessage<T> right) => !(left == right);
    }
}
