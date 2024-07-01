using MemoryPack;
using R3;

namespace Some1.Sync.Sources
{
    public sealed class UnmanagedParticleSource<T> : SyncParticleSource<T> where T : unmanaged
    {
        public UnmanagedParticleSource(ReadOnlyReactiveProperty<T> value) : base(value)
        {
        }

        protected sealed override void WriteValue<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer)
            => writer.WriteUnmanaged(Value.CurrentValue);
    }
}
