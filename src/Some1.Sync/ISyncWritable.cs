using System.Buffers;
using MemoryPack;

namespace Some1.Sync
{
    public interface ISyncWritable
    {
        void Write<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, SyncMode mode) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>;
    }
}
