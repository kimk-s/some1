using System;

namespace Some1.Sync
{
    public readonly struct SyncPipeWriteResult : IEquatable<SyncPipeWriteResult>
    {
        public SyncPipeWriteResult(bool isPending, bool isCompleted)
        {
            IsPending = isPending;
            IsCompleted = isCompleted;
        }

        public static SyncPipeWriteResult None => default;

        public bool IsPending { get; }

        public bool IsCompleted { get; }

        public override bool Equals(object? obj) => obj is SyncPipeWriteResult other && Equals(other);

        public bool Equals(SyncPipeWriteResult other) => IsPending == other.IsPending && IsCompleted == other.IsCompleted;

        public override int GetHashCode() => HashCode.Combine(IsPending, IsCompleted);

        public static bool operator ==(SyncPipeWriteResult left, SyncPipeWriteResult right) => left.Equals(right);

        public static bool operator !=(SyncPipeWriteResult left, SyncPipeWriteResult right) => !(left == right);
    }
}
