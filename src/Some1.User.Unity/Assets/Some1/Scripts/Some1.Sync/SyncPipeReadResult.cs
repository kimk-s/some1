using System;

namespace Some1.Sync
{
    public readonly struct SyncPipeReadResult : IEquatable<SyncPipeReadResult>
    {
        public SyncPipeReadResult(int bodyReadCount, bool isCompleted)
        {
            BodyReadCount = bodyReadCount;
            IsCompleted = isCompleted;
        }

        public static SyncPipeReadResult None => default;

        public int BodyReadCount { get; }

        public bool IsCompleted { get; }

        public override bool Equals(object? obj) => obj is SyncPipeReadResult other && Equals(other);

        public bool Equals(SyncPipeReadResult other) => BodyReadCount == other.BodyReadCount && IsCompleted == other.IsCompleted;

        public override int GetHashCode() => HashCode.Combine(BodyReadCount, IsCompleted);

        public static bool operator ==(SyncPipeReadResult left, SyncPipeReadResult right) => left.Equals(right);

        public static bool operator !=(SyncPipeReadResult left, SyncPipeReadResult right) => !(left == right);
    }
}
