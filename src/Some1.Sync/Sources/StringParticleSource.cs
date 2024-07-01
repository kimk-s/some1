using MemoryPack;
using R3;

namespace Some1.Sync.Sources
{
    public sealed class StringParticleSource : SyncParticleSource<string?>
    {
        public StringParticleSource(ReadOnlyReactiveProperty<string?> value) : base(value)
        {
        }

        protected sealed override void WriteValue<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer)
            => writer.WriteString(Value.CurrentValue);
    }
}
