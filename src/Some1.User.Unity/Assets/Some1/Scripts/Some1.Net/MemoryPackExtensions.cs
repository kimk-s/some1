using System.Buffers;
using System.Runtime.CompilerServices;
using MemoryPack;

namespace Some1.Net
{
    public static class MemoryPackExtensions
    {
        public static ref TValue WriteUnmanagedReference<TBufferWriter, TValue>(this ref MemoryPackWriter<TBufferWriter> writer, TValue value = default)
            where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
            where TValue : unmanaged
        {
            ref byte spanReference = ref writer.GetSpanReference(Unsafe.SizeOf<TValue>());
            ref TValue result = ref Unsafe.As<byte, TValue>(ref spanReference);
            result = value;
            writer.Advance(Unsafe.SizeOf<TValue>());
            return ref result;
        }
    }
}
