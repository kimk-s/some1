using MemoryPack;

namespace Some1.Sync
{
    public interface ISyncReadable
    {
        void Read(ref MemoryPackReader reader, SyncMode mode);
    }
}
