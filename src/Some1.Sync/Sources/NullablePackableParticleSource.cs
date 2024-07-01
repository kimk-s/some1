using System;
using MemoryPack;
using R3;

namespace Some1.Sync.Sources
{
    public sealed class NullablePackableParticleSource<T> : SyncParticleSource<T?> where T : struct, IMemoryPackable<T>
    {
        public NullablePackableParticleSource(ReadOnlyReactiveProperty<T?> value) : base(value)
        {
        }

        protected sealed override void WriteValue<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer)
        {
            if (!Value.CurrentValue.HasValue)
            {
                throw new InvalidOperationException();
            }

            writer.WritePackable(Value.CurrentValue.Value);
        }
    }
}
