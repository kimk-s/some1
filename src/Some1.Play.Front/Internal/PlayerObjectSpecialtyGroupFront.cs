using System;
using System.Collections.Generic;
using System.Linq;
using MemoryPack;
using R3;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerObjectSpecialtyGroupFront : IPlayerObjectSpecialtyGroupFront, ISyncDestination
    {
        private readonly SyncArrayDestination _sync;
        private readonly PlayerObjectSpecialtyFront[] _all;
        private readonly ReactiveProperty<int> _activeCount = new();

        internal PlayerObjectSpecialtyGroupFront()
        {
            _all = Enumerable.Range(0, PlayConst.SpecialtyCount)
                .Select(x => new PlayerObjectSpecialtyFront())
                .ToArray();

            _sync = new SyncArrayDestination(_all);

            _all.Select(x => x.Specialty.Select(y => y is not null))
                .Merge()
                .Subscribe(_ =>
                {
                    int activeCount = 0;

                    foreach (var x in _all.AsSpan())
                    {
                        if (x.Specialty.CurrentValue is not null)
                        {
                            activeCount++;
                        }
                    }

                    _activeCount.Value = activeCount;
                });

        }

        public IReadOnlyList<IPlayerObjectSpecialtyFront> All => _all;

        public int PinnedCount
        {
            get
            {
                int count = 0;
                foreach (var item in _all)
                {
                    if (item.Specialty.CurrentValue?.IsPinned == true)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        public ReadOnlyReactiveProperty<int> ActiveCount => _activeCount;

        public void Dispose()
        {
            _sync.Dispose();
            _activeCount.Dispose();
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            _sync.Read(ref reader, mode);
        }

        public void Reset()
        {
            _sync.Reset();
        }
    }
}
