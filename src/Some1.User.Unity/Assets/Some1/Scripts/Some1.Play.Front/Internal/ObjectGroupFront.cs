using System;
using System.Collections.Generic;
using MemoryPack;
using ObservableCollections;
using Some1.Play.Info;
using Some1.Sync;

namespace Some1.Play.Front.Internal
{
    internal sealed class ObjectGroupFront : IObjectGroupFront, ISyncReadable, ISyncPolatable
    {
        private readonly ObservableList<IObjectFront> _all = new();
        private readonly Dictionary<int, IObjectFront> _activeItems = new();
        private readonly Stack<ObjectFront> _pool = new();
        private readonly ISyncTime _syncFrame;
        private readonly PlayInfo _info;
        private readonly IPlayerObjectFront _playerObject;

        internal ObjectGroupFront(ISyncTime syncFrame, PlayInfo info, IPlayerObjectFront playerObject)
        {
            _syncFrame = syncFrame;
            _info = info;
            _playerObject = playerObject;
        }

        public IObservableCollection<IObjectFront> All => _all;

        public IReadOnlyDictionary<int, IObjectFront> ActiveItems => _activeItems;

        public void Dispose()
        {
            foreach (var item in _all)
            {
                ((IDisposable)item).Dispose();
            }
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            if (mode == SyncMode.Full)
            {
                ClearActiveItems();

                int count = reader.ReadUnmanaged<int>();
                for (int i = 0; i < count; i++)
                {
                    var item = New();
                    item.Read(ref reader, mode);
                    AddActiveItem(item);
                }
            }
            else
            {
                int updateCount = reader.ReadUnmanaged<int>();
                for (int i = 0; i < updateCount; i++)
                {
                    int id = reader.ReadUnmanaged<int>();
                    var item = GetActiveItem(id);
                    item.Read(ref reader, SyncMode.Dirty);
                }

                int removeCount = reader.ReadUnmanaged<int>();
                for (int i = 0; i < removeCount; i++)
                {
                    int id = reader.ReadUnmanaged<int>();
                    RemoveActiveItem(id);
                }

                int addCount = reader.ReadUnmanaged<int>();
                for (int i = 0; i < addCount; i++)
                {
                    var item = New();
                    item.Read(ref reader, SyncMode.Full);
                    AddActiveItem(item);
                }
            }
        }

        public void Reset()
        {
            foreach (var item in _activeItems.Values)
            {
                ((ObjectFront)item).Reset();
                _pool.Push((ObjectFront)item);
            }
            _activeItems.Clear();
        }

        public void Extrapolate()
        {
            foreach (var item in _activeItems.Values)
            {
                ((ObjectFront)item).Extrapolate();
            }
        }

        public void Interpolate(float time)
        {
            foreach (var item in _activeItems.Values)
            {
                ((ObjectFront)item).Interpolate(time);
            }
        }

        internal void Update(float deltaSeconds)
        {
            foreach (var item in _activeItems.Values)
            {
                ((ObjectFront)item).Update(deltaSeconds);
            }
        }

        private ObjectFront New()
        {
            if (!_pool.TryPop(out var result))
            {
                result = new(
                    _syncFrame,
                    _info.Characters,
                    _info.CharacterAlives,
                    _info.CharacterIdles,
                    _info.CharacterCasts,
                    _info.CharacterSkins,
                    _info.CharacterSkinEmojis,
                    _info.BuffSkins,
                    _info.Boosters,
                    _playerObject);
                _all.Add(result);
            }

            return result;
        }

        private ObjectFront GetActiveItem(int id)
        {
            return (ObjectFront)_activeItems[id];
        }

        private void AddActiveItem(ObjectFront item)
        {
            if (!item.Active.CurrentValue)
            {
                throw new InvalidOperationException();
            }

            _activeItems.Add(item.Id.CurrentValue, item);
        }

        private void RemoveActiveItem(int id)
        {
            if (!_activeItems.Remove(id, out var item))
            {
                throw new InvalidOperationException("1");
            }

            if (item.Id.CurrentValue != id)
            {
                throw new InvalidOperationException("2");
            }

            ((ObjectFront)item).Reset();
            _pool.Push((ObjectFront)item);
        }

        private void ClearActiveItems()
        {
            foreach (var item in _activeItems.Values)
            {
                ((ObjectFront)item).Reset();
                _pool.Push((ObjectFront)item);
            }
            _activeItems.Clear();
        }
    }
}
