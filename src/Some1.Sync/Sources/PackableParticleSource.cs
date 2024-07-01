using MemoryPack;
using R3;

namespace Some1.Sync.Sources
{
    public sealed class PackableParticleSource<T> : SyncParticleSource<T?> where T : IMemoryPackable<T>
    {
        public PackableParticleSource(ReadOnlyReactiveProperty<T?> value) : base(value)
        {
        }

        protected sealed override void WriteValue<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer)
            => writer.WritePackable(Value.CurrentValue);
    }
}
