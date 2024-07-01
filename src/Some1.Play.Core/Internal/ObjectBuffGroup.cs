using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using MemoryPack;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;
using R3;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectBuffGroup : ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly BuffInfoGroup _infos;
        private readonly int _rootId;
        private readonly ObjectBuff[] _all;
        private int _lastToken;

        internal ObjectBuffGroup(
            int count,
            BuffInfoGroup infos,
            BuffStatInfoGroup statInfos,
            ObjectHitGroup hits,
            ObjectStatGroup stats,
            int rootId,
            ObjectTrait trait,
            ITime time)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (rootId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rootId));
            }

            _infos = infos;
            _rootId = rootId;
            _all = Enumerable.Range(0, count)
                .Select(x => new ObjectBuff(
                    x,
                    infos,
                    statInfos,
                    hits,
                    stats,
                    trait,
                    time))
                .ToArray();
            _sync = new SyncArraySource(_all);
        }

        internal IReadOnlyList<IObjectBuff> All => _all;

        internal bool CanAdd => Enabled;

        internal bool Enabled { get; set; }

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal bool Add(BuffId id, int rootId, float angle, SkinId skinId, int offenceStat, Trait traitId, byte team)
        {
            if (!CanAdd)
            {
                return false;
            }

            var item = _infos.ById[id].IsUnique
                ? Get(id, rootId) ?? GetItemToSet(rootId)
                : GetItemToSet(rootId);

            item?.Set(id, rootId, angle, skinId, offenceStat, traitId, team, GenerateToken());

            return true;
        }

        internal void Update(float deltaSeconds, ParallelToken parallelToken)
        {
            if (!Enabled)
            {
                return;
            }

            foreach (var item in _all.AsSpan())
            {
                item.Update(deltaSeconds, parallelToken);
            }
        }

        internal void Reset()
        {
            if (!Enabled)
            {
                return;
            }

            Stop();
            Enabled = false;
        }

        internal void Stop()
        {
            if (!Enabled)
            {
                return;
            }

            foreach (var item in _all.AsSpan())
            {
                item.Reset();
            }
            _lastToken = 0;
        }

        internal void Stop(BuffId id)
        {
            foreach (var item in _all.AsSpan())
            {
                if (item.Id == id)
                {
                    item.Reset();
                    break;
                }
            }
        }

        private int GenerateToken()
        {
            return ++_lastToken;
        }

        private ObjectBuff? GetItemToSet(int rootId)
        {
            ObjectBuff? result = null;
            bool isMine = rootId == _rootId;

            foreach (var item in _all.AsSpan())
            {
                if ((isMine || _rootId != item.RootId)
                    && (result is null || result.Token > item.Token))
                {
                    result = item;
                }
            }

            return result;
        }

        private ObjectBuff? Get(BuffId id, int rootId)
        {
            foreach (var item in _all.AsSpan())
            {
                if (item.Id is not null && item.Id == id && item.RootId == rootId)
                {
                    return item;
                }
            }
            return null;
        }

        public void ClearDirty()
        {
            _sync.ClearDirty();
        }

        public void Write<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, SyncMode mode) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
        {
            _sync.Write(ref writer, mode);
        }

        public void Dispose()
        {
            _sync.Dispose();
        }
    }
}
