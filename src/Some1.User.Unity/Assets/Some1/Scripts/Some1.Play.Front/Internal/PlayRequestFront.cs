using System;
using System.Buffers;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayRequestFront : ISyncWritable, IDisposable
    {
        private readonly SyncArraySource _sync;

        public PlayRequestFront(
            ReadOnlyReactiveProperty<Unary?> unary,
            ReadOnlyReactiveProperty<Cast?> cast,
            ReadOnlyReactiveProperty<Walk?> walk)
        {
            _sync = new SyncArraySource(
                unary.ToNullablePackableParticleSource(),
                cast.ToNullableUnmanagedParticleSource(),
                walk.ToNullableUnmanagedParticleSource());
        }

        public void ClearDirty()
        {
            _sync.ClearDirty();
        }

        public void Dispose()
        {
            _sync.Dispose();
        }

        public void Write<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, SyncMode mode) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
        {
            writer.WriteSourceDirtySafely(_sync, mode);
        }
    }
}
