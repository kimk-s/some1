using System;
using System.Collections.Generic;
using System.Linq;
using MemoryPack;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class ObjectLikeGroupFront : ISyncDestination
    {
        private const int ItemCount = 10;

        private readonly UnmanagedParticleDestination<int> _count = new();
        private readonly ObjectLikeFront[] _all;
        private readonly List<ObjectLikeFront> _setItems;
        private readonly ReadOnlyReactiveProperty<int> _objectId;
        private int _previousObjectId;
        private int _previousCount;

        internal ObjectLikeGroupFront(ReadOnlyReactiveProperty<int> objectId)
        {
            _all = Enumerable.Range(0, ItemCount)
                .Select(_ => new ObjectLikeFront())
                .ToArray();
            _setItems = new(_all.Length);
            _objectId = objectId;
        }

        public IReadOnlyList<IObjectLikeFront> All => _all;

        public ReadOnlyReactiveProperty<bool> IsDefault => _count.IsDefault;

        public void Dispose()
        {
            _count.Dispose();
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            _count.Read(ref reader, mode);
        }

        public void Reset()
        {
            _count.Reset();
        }

        internal void Update(float deltaSeconds)
        {
            if (_previousObjectId == _objectId.CurrentValue)
            {
                for (int i = _setItems.Count - 1; i >= 0; i--)
                {
                    var item = _setItems[i];

                    item.Update(deltaSeconds);

                    if (item.Cycles.CurrentValue > 1)
                    {
                        ResetItem(i);
                    }
                }

                if (_previousCount != _count.Value.CurrentValue)
                {
                    int length = Math.Min(_count.Value.CurrentValue - _previousCount, _all.Length);

                    for (int i = 0; i < length; i++)
                    {
                        SetItem(GetItem());
                    }
                }
            }
            else
            {
                ResetItemAll();
            }

            _previousObjectId = _objectId.CurrentValue;
            _previousCount = _count.Value.CurrentValue;
        }

        private ObjectLikeFront GetItem()
        {
            ObjectLikeFront oldest = null!;

            foreach (var item in _all)
            {
                if (!item.Value.CurrentValue)
                {
                    return item;
                }

                if (oldest is null || oldest.Cycles.CurrentValue < item.Cycles.CurrentValue)
                {
                    oldest = item;
                }
            }

            return oldest;
        }

        private void SetItem(ObjectLikeFront item)
        {
            item.Set();
            _setItems.Add(item);
        }

        private void ResetItem(int setItemIndex)
        {
            _setItems[setItemIndex].Reset();
            _setItems.RemoveAt(setItemIndex);
        }

        private void ResetItemAll()
        {
            int length = _setItems.Count;
            for (int i = length - 1; i >= 0; i--)
            {
                ResetItem(i);
            }
        }
    }
}
