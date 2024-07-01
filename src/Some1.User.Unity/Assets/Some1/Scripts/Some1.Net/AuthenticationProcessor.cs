using System;
using System.Buffers;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using MemoryPack;

namespace Some1.Net
{
    public sealed class AuthenticationProcessor : IDisposable
    {
        private const int ReceiveBufferSize = 8_192;

        private readonly Socket _socket;
        private readonly IMemoryOwner<byte> _memoryOwner;
        private readonly BufferWriter _bufferWriter;

        public AuthenticationProcessor(Socket socket)
        {
            _socket = socket;
            _memoryOwner = MemoryPool<byte>.Shared.Rent(ReceiveBufferSize);
            _bufferWriter = new(_memoryOwner.Memory);
        }

        public async Task SendAsync<T>(T value, CancellationToken cancellationToken) where T : IMemoryPackable<T>
        {
            if (_bufferWriter.WrittenCount != 0)
            {
                throw new InvalidOperationException();
            }

            Write(_bufferWriter, value);

            await _socket.SendAsync(_bufferWriter.WrittenBuffer, SocketFlags.None, cancellationToken).ConfigureAwait(false);

            static void Write(BufferWriter buffer, T value)
            {
                using var state = MemoryPackWriterOptionalStatePool.Rent(MemoryPackSerializerOptions.Default);
                var writer = new MemoryPackWriter<BufferWriter>(ref buffer, state);
                ref int header = ref writer.WriteUnmanagedReference<BufferWriter, int>();
                writer.WritePackable(value);
                header = writer.WrittenCount - sizeof(int);
                writer.Flush();
            }
        }

        public async Task<T> ReadAsync<T>(CancellationToken cancellationToken) where T : IMemoryPackable<T>
        {
            var memory = _memoryOwner.Memory;

            var headerMemory = memory[..sizeof(int)];
            await ReceiveAsync(_socket, headerMemory, cancellationToken).ConfigureAwait(false);
            int header = ReadHeader(headerMemory.Span);

            Memory<byte> bodyMemory;
            try
            {
                bodyMemory = memory.Slice(sizeof(int), header);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new InvalidOperationException($"Header: {header}");
            }
            await ReceiveAsync(_socket, bodyMemory, cancellationToken).ConfigureAwait(false);
            return ReadBody(bodyMemory.Span);

            static async Task ReceiveAsync(Socket socket, Memory<byte> memory, CancellationToken cancellationToken)
            {
                int readCount = 0;
                while (readCount < memory.Length)
                {
#if UNITY
                    if (!MemoryMarshal.TryGetArray<byte>(memory[readCount..], out var segment))
                    {
                        throw new InvalidOperationException();
                    }
                    int bytesRead = await socket.ReceiveAsync(segment, SocketFlags.None).ConfigureAwait(false);
#else
                    int bytesRead = await socket.ReceiveAsync(memory[readCount..], SocketFlags.None, cancellationToken).ConfigureAwait(false);
#endif
                    if (bytesRead == 0)
                    {
                        throw new InvalidOperationException("Bytes read is zero");
                    }

                    readCount += bytesRead;
                }
            }

            static int ReadHeader(Span<byte> span)
            {
                return Unsafe.As<byte, int>(ref MemoryMarshal.GetReference(span));
            }

            static T ReadBody(Span<byte> span)
            {
                using var state = MemoryPackReaderOptionalStatePool.Rent(MemoryPackSerializerOptions.Default);
                var reader = new MemoryPackReader(span, state);
                return reader.ReadPackable<T>() ?? throw new InvalidOperationException();
            }
        }

        public void Dispose()
        {
            _memoryOwner.Dispose();
        }

        private sealed class BufferWriter : IBufferWriter<byte>
        {
            private readonly Memory<byte> _buffer;

            public BufferWriter(Memory<byte> buffer)
            {
                _buffer = buffer;
            }

            public int WrittenCount { get; private set; }

            public int RemainingCount => _buffer.Length - WrittenCount;

            public Memory<byte> WrittenBuffer => _buffer[..WrittenCount];

            public void Advance(int count)
            {
                if (count > RemainingCount)
                {
                    throw new InvalidOperationException();
                }

                WrittenCount += count;
            }

            public Memory<byte> GetMemory(int sizeHint = 0)
            {
                if (sizeHint > RemainingCount)
                {
                    throw new ArgumentOutOfRangeException(nameof(sizeHint));
                }

                return _buffer.Slice(WrittenCount, RemainingCount);
            }

            public Span<byte> GetSpan(int sizeHint = 0)
            {
                if (sizeHint > RemainingCount)
                {
                    throw new ArgumentOutOfRangeException(nameof(sizeHint));
                }

                return _buffer.Span.Slice(WrittenCount, RemainingCount);
            }
        }
    }
}
