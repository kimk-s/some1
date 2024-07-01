using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Compression;
using MemoryPack;
using MemoryPack.Compression;
using Some1.Sync.Destinations;
using Some1.Sync.Sources;
using R3;
using System.Numerics;

namespace Some1.Sync
{
    public static class SyncExtensions
    {
        public static UnmanagedParticleSource<T> ToUnmanagedParticleSource<T>(this ReadOnlyReactiveProperty<T> value) where T : unmanaged
            => new(value);

        public static NullableUnmanagedParticleSource<T> ToNullableUnmanagedParticleSource<T>(this ReadOnlyReactiveProperty<T?> value) where T : unmanaged
            => new(value);

        public static StringParticleSource ToStringParticleSource(this ReadOnlyReactiveProperty<string?> value)
            => new(value);

        public static PackableParticleSource<T> ToPackableParticleSource<T>(this ReadOnlyReactiveProperty<T?> value) where T : IMemoryPackable<T>
            => new(value);

        public static NullablePackableParticleSource<T> ToNullablePackableParticleSource<T>(this ReadOnlyReactiveProperty<T?> value) where T : struct, IMemoryPackable<T>
            => new(value);

        public static ValueParticleSource<T?> ToValueParticleSource<T>(this ReadOnlyReactiveProperty<T?> value)
            => new(value);

        public static FloatWaveSource ToWaveSource(this ReadOnlyReactiveProperty<FloatWave> value, ISyncTime time)
            => new(value, time);

        public static FloatWaveSource ToWaveSource(this ReadOnlyReactiveProperty<FloatWave> value, ISyncTime time, float tolerance)
            => new(value, time, tolerance);

        public static DoubleWaveSource ToWaveSource(this ReadOnlyReactiveProperty<DoubleWave> value, ISyncTime time)
            => new(value, time);

        public static DoubleWaveSource ToWaveSource(this ReadOnlyReactiveProperty<DoubleWave> value, ISyncTime time, double tolerance)
            => new(value, time, tolerance);

        public static Vector2WaveSource ToWaveSource(this ReadOnlyReactiveProperty<Vector2Wave> value, ISyncTime time)
            => new(value, time);

        public static Vector2WaveSource ToWaveSource(this ReadOnlyReactiveProperty<Vector2Wave> value, ISyncTime time, Vector2 tolerance)
            => new(value, time, tolerance);

        public static SyncArraySource ToArraySource(this IEnumerable<ISyncSource> items) => new(items);

        public static SyncArraySource ToArraySource(this ISyncSource[] items) => new(items);

        public static SyncArrayDestination ToArrayDestination(this IEnumerable<ISyncDestination> items) => new(items);

        public static SyncArrayDestination ToArrayDestination(this ISyncDestination[] items) => new(items);

        public static void WriteSourceDirtySafely<TBufferWriter>(this ref MemoryPackWriter<TBufferWriter> writer, ISyncSource source, SyncMode mode) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
        {
            if (mode == SyncMode.Full)
            {
                source.Write(ref writer, mode);
            }
            else
            {
                if (!source.Dirty.CurrentValue)
                {
                    writer.WriteUnmanaged((byte)0);
                }
                else
                {
                    writer.WriteUnmanaged((byte)1);
                    source.Write(ref writer, mode);
                }
            }
        }

        public static void ReadDestinationDirtySafely(this ref MemoryPackReader reader, ISyncDestination destination, SyncMode mode)
        {
            if (mode == SyncMode.Full)
            {
                destination.Read(ref reader, mode);
            }
            else
            {
                byte code = reader.ReadUnmanaged<byte>();
                if (code == 0)
                {
                }
                else if (code == 1)
                {
                    destination.Read(ref reader, mode);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public static void WriteSyncWithCompression<TBufferWriter>(this ref MemoryPackWriter<TBufferWriter> writer, ISyncWritable syncWritable, SyncMode syncMode) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
        {
            WriteSyncWithCompression(ref writer, syncWritable, syncMode, CompressionLevel.Fastest);
        }

        public static void WriteSyncWithCompression<TBufferWriter>(this ref MemoryPackWriter<TBufferWriter> writer, ISyncWritable syncWritable, SyncMode syncMode, CompressionLevel compressionLevel)where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
        {
            const int window = 22; // MemoryPack.Compression.BrotliUtils.WindowBits_Default
            WriteSyncWithCompression(ref writer, syncWritable, syncMode, compressionLevel, window);
        }

        public static void WriteSyncWithCompression<TBufferWriter>(this ref MemoryPackWriter<TBufferWriter> writer, ISyncWritable syncWritable, SyncMode syncMode, CompressionLevel compressionLevel, int window) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
        {
            var compressor = new BrotliCompressor(compressionLevel, window);
            
            try
            {
                var coWriter = new MemoryPackWriter<BrotliCompressor>(ref compressor, writer.OptionalState);
                syncWritable.Write(ref coWriter, syncMode);
                coWriter.Flush();

                compressor.CopyTo(ref writer);
            }
            finally
            {
                compressor.Dispose();
            }
        }

        public static void ReadSyncWithCompression(this ref MemoryPackReader reader, ISyncReadable syncReadable, SyncMode syncMode)
        {
            using var decompressor = new BrotliDecompressor();

            reader.GetRemainingSource(out var singleSource, out var remainingSource);

            int consumed;
            ReadOnlySequence<byte> decompressedSource;
            if (singleSource.Length != 0)
            {
                decompressedSource = decompressor.Decompress(singleSource, out consumed);
            }
            else
            {
                decompressedSource = decompressor.Decompress(remainingSource, out consumed);
            }

            var coReader = new MemoryPackReader(decompressedSource, reader.OptionalState);
            try
            {
                syncReadable.Read(ref coReader, syncMode);
            }
            finally
            {
                coReader.Dispose();
            }

            reader.Advance(consumed);
        }
    }
}
