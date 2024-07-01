using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using MemoryPack;
using Some1.Net;

namespace Some1.Sync
{
    public static class MemoryPackReaderExtensions
    {
        public static bool TryReadSyncHeader(this ref MemoryPackReader reader, out SyncHeader header)
        {
            int headerSize = Unsafe.SizeOf<SyncHeader>();
            if (reader.Remaining < headerSize)
            {
                header = default;
                return false;
            }

            reader.ReadUnmanaged(out header);
            return true;
        }

        public static bool TryReadSyncBody(this ref MemoryPackReader reader)
        {
            int headerSize = Unsafe.SizeOf<SyncHeader>();
            if (reader.Remaining < headerSize)
            {
                return false;
            }

            var header = Unsafe.ReadUnaligned<SyncHeader>(ref reader.GetSpanReference(headerSize));
            if (reader.Remaining < headerSize + header.bodySize)
            {
                return false;
            }

            reader.Advance(headerSize);
            return true;
        }
    }

    public sealed class SyncPipe
    {
        private readonly ISyncReadable _bodyReadable;
        private readonly ISyncWritable _bodyWritable;
        private DuplexPipe? _pipe;
        private bool _read;
        private bool _written;

        public SyncPipe(ISyncReadable bodyReadable, ISyncWritable bodyWritable)
        {
            _bodyReadable = bodyReadable;
            _bodyWritable = bodyWritable;
        }

        public bool IsPipeNull => Pipe is null;

        public bool IsInputCompleted { get; private set; }

        public bool IsOutputCompleted { get; private set; }

        private DuplexPipe? Pipe
        {
            get => _pipe;

            set
            {
                if (_pipe == value)
                {
                    return;
                }

                Complete();

                _pipe = value;

                _read = false;
                _written = false;
                IsInputCompleted = false;
                IsOutputCompleted = false;
            }
        }

        public void SetPipe(DuplexPipe pipe)
        {
            if (pipe is null)
            {
                throw new ArgumentNullException(nameof(pipe));
            }

            Pipe = pipe;
        }

        public SyncPipeReadResult Read(int maxCount)
        {
            if (Pipe is null)
            {
                return SyncPipeReadResult.None;
            }

            if (!Pipe.Input.TryRead(out var result))
            {
                return SyncPipeReadResult.None;
            }

            var buffer = result.Buffer;
            using var state = MemoryPackReaderOptionalStatePool.Rent(MemoryPackSerializerOptions.Default);
            var reader = new MemoryPackReader(buffer, state);
            int count = 0;

            try
            {
                while (reader.TryReadSyncBody())
                {
                    var mode = _read ? SyncMode.Dirty : SyncMode.Full;

                    _bodyReadable.Read(ref reader, mode);

                    _read = true;
                    (_bodyReadable as ISyncPolatable)?.Extrapolate();

                    if (++count >= maxCount)
                    {
                        break;
                    }
                }

                buffer = buffer.Slice(reader.Consumed);
            }
            finally
            {
                reader.Dispose();
            }

            Pipe.Input.AdvanceTo(buffer.Start);

            if (result.IsCanceled)
            {
                throw new NotSupportedException();
            }

            return new(count, result.IsCompleted);
        }

        public float GetReadableDeltaTime(float maxDeltaTime)
        {
            if (Pipe is null)
            {
                return 0;
            }

            if (!Pipe.Input.TryRead(out var result))
            {
                return 0;
            }

            var buffer = result.Buffer;
            using var state = MemoryPackReaderOptionalStatePool.Rent(MemoryPackSerializerOptions.Default);
            var reader = new MemoryPackReader(buffer, state);
            float deltaTime = 0;

            try
            {
                while (reader.TryReadSyncHeader(out var header))
                {
                    if (header.deltaTime <= 0)
                    {
                        throw new InvalidOperationException($"Invalid Header DeltaTime ({header.deltaTime}).");
                    }

                    deltaTime += header.deltaTime;

                    if (deltaTime >= maxDeltaTime)
                    {
                        break;
                    }

                    if (reader.Remaining < header.bodySize)
                    {
                        break;
                    }

                    reader.Advance(header.bodySize);
                }
            }
            finally
            {
                reader.Dispose();
            }

            Pipe.Input.AdvanceTo(buffer.Start);

            return deltaTime;
        }

        public SyncPipeWriteResult Write(MemoryPackSerializerOptions? options = null)
        {
            if (Pipe is null)
            {
                return SyncPipeWriteResult.None;
            }

            var buffer = Pipe.Output;
            using var state = MemoryPackWriterOptionalStatePool.Rent(options);
            var writer = new MemoryPackWriter<PipeWriter>(ref buffer, state);
            ref var header = ref writer.WriteUnmanagedReference<PipeWriter, SyncHeader>();
            var mode = _written ? SyncMode.Dirty : SyncMode.Full;

            _bodyWritable.Write(ref writer, mode);

            _written = true;
            header.bodySize = writer.WrittenCount - Unsafe.SizeOf<SyncHeader>();
            header.deltaTime = (_bodyWritable as ISyncTime)?.DeltaTime.B ?? 0;
            writer.Flush();

            var resultTask = Pipe.Output.FlushAsync();
            if (!resultTask.IsCompleted)
            {
                return new(true, false);
            }

            var result = resultTask.Result;
            if (result.IsCanceled)
            {
                throw new NotSupportedException();
            }

            return new(false, result.IsCompleted);
        }

        public void Complete(Exception? inputException = null, Exception? outputException = null)
        {
            CompleteInput(inputException);
            CompleteOutput(outputException);
        }

        public void CompleteInput(Exception? exception = null)
        {
            if (Pipe is null || IsInputCompleted)
            {
                return;
            }

            Pipe.Input.Complete(exception);
            IsInputCompleted = true;
            Reset();
        }

        public void CompleteOutput(Exception? exception = null)
        {
            if (Pipe is null || IsOutputCompleted)
            {
                return;
            }

            Pipe.Output.Complete(exception);
            IsOutputCompleted = true;
            Reset();
        }

        private void Reset()
        {
            if (Pipe is null || !IsInputCompleted || !IsOutputCompleted)
            {
                return;
            }

            Pipe.Reset();
            Pipe = null;
        }

        private static bool TryReadPacket(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> packet)
        {
            var reader = new SequenceReader<byte>(buffer);
            if (reader.TryReadLittleEndian(out int header) && buffer.Length >= sizeof(int) + header)
            {
                packet = buffer.Slice(0, sizeof(int) + header);
                buffer = buffer.Slice(sizeof(int) + header);
                return true;
            }

            packet = default;
            return false;
        }
    }
}
