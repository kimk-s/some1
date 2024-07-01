using System;
using System.Buffers;
using System.Collections.Generic;
using MemoryPack;
using Some1.Net;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;
using Some1.Sync;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectGroup : ISyncWritable
    {
        private readonly IObject _looker;
        private readonly Space _space;
        private readonly Dictionary<int, Object> _objects = new();
        private readonly List<int> _idToRemove = new();
        private ParallelToken? _parallelToken;

        public ObjectGroup(IObject looker, Space space)
        {
            _looker = looker;
            _space = space;
        }

        public void PrepareWrite(ParallelToken parallelToken)
        {
            _parallelToken = parallelToken;
        }

        private void ClearWritePrepared()
        {
            _parallelToken = null;
        }

        public void Write<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, SyncMode mode) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
        {
            var parallelToken = _parallelToken ?? throw new InvalidOperationException();

            var looker = new Looker(
                Area.Rectangle(
                    _looker.Properties.Area.Position + PlayConst.PlayerEyesightPosition,
                    PlayConst.PlayerEyesightSize),
                _looker.Properties.Team.CurrentValue);

            if (mode == SyncMode.Full)
            {
                _objects.Clear();
                ref int count = ref writer.WriteUnmanagedReference<TBufferWriter, int>();
                foreach (var item in _space.GetObjects(looker.Area, parallelToken))
                {
                    if (item.Look(looker))
                    {
                        _objects.Add(item.Id, item);
                        item.Write(ref writer, SyncMode.Full);
                        count++;
                    }
                }
            }
            else
            {
                // Update
                {
                    ref int count = ref writer.WriteUnmanagedReference<TBufferWriter, int>();
                    foreach (var item in _objects.Values)
                    {
                        if (item.Look(looker))
                        {
                            if (item.Dirty.CurrentValue)
                            {
                                writer.WriteUnmanaged(item.Id);
                                item.Write(ref writer, SyncMode.Dirty);
                                count++;
                            }
                        }
                        else
                        {
                            _idToRemove.Add(item.Id);
                        }
                    }
                }

                // Remove
                {
                    writer.WriteUnmanaged(_idToRemove.Count);
                    foreach (var item in _idToRemove)
                    {
                        writer.WriteUnmanaged(item);
                        _objects.Remove(item);
                    }
                    _idToRemove.Clear();
                }

                // Add
                {
                    ref int count = ref writer.WriteUnmanagedReference<TBufferWriter, int>();
                    foreach (var item in _space.GetObjects(looker.Area, parallelToken))
                    {
                        if (item.Look(looker) && _objects.TryAdd(item.Id, item))
                        {
                            item.Write(ref writer, SyncMode.Full);
                            count++;
                        }
                    }
                }
            }

            ClearWritePrepared();
        }
    }
}
