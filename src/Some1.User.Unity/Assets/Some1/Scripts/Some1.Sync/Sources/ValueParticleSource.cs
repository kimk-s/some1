using MemoryPack;
using R3;

namespace Some1.Sync.Sources
{
    public sealed class ValueParticleSource<T> : SyncParticleSource<T?>
    {
        public ValueParticleSource(ReadOnlyReactiveProperty<T?> value) : base(value)
        {
        }

        protected sealed override void WriteValue<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer)
            => writer.WriteValue(Value.CurrentValue);
    }
}
