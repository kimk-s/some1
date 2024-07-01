using System;
using System.Buffers;
using MemoryPack;
using Some1.Sync;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class PlayResponse : ISyncWritable, ISyncTime, IDisposable
    {
        private readonly SyncArraySource _player;
        private readonly ITime _time;
        private readonly Writable _writable;

        public PlayResponse(SyncArraySource player, RankingGroup rankings, ObjectGroup objects, ITime time)
        {
            _player = player;
            _time = time;
            _writable = new(player, rankings, objects, time);
        }

        public int FrameCount => _time.FrameCount;

        public FloatWave DeltaTime => _time.DeltaTime;

        public FloatWave FPS => _time.FPS;

        public void ClearDirty()
        {
            _player.ClearDirty();
        }

        public void Dispose()
        {
            _player.Dispose();
        }

        public void Write<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, SyncMode mode) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
        {
            //_writable.Write(ref writer, mode);
            writer.WriteSyncWithCompression(_writable, mode);
        }

        private sealed class Writable : ISyncWritable
        {
            private readonly SyncArraySource _player;
            private readonly RankingGroup _rankings;
            private readonly ObjectGroup _objects;
            private readonly ISyncWritable _time;

            public Writable(SyncArraySource player, RankingGroup rankings, ObjectGroup objects, ITime time)
            {
                _player = player;
                _rankings = rankings;
                _objects = objects;
                _time = (ISyncWritable)time;
            }

            public void Write<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, SyncMode mode) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
            {
                _time.Write(ref writer, mode);
                _objects.Write(ref writer, mode);
                _rankings.Write(ref writer, mode);
                writer.WriteSourceDirtySafely(_player, mode);
            }
        }
    }
}
