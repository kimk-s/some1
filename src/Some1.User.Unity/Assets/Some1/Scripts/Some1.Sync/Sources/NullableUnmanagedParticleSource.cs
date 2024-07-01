using System;
using MemoryPack;
using R3;

namespace Some1.Sync.Sources
{
    public sealed class NullableUnmanagedParticleSource<T> : SyncParticleSource<T?> where T : unmanaged
    {
        public NullableUnmanagedParticleSource(ReadOnlyReactiveProperty<T?> value) : base(value)
        {
        }

        protected sealed override void WriteValue<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer)
        {
            if (!Value.CurrentValue.HasValue)
            {
                throw new InvalidOperationException();
            }

            writer.WriteUnmanaged(Value.CurrentValue.Value);
        }
    }
}
