using System;
using System.Collections.Generic;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class RankingGroupFront : IRankingGroupFront, ISyncReadable, IDisposable
    {
        private readonly SyncArrayDestination _sync;
        private readonly RankingFront[] _all;
        private readonly ReactiveProperty<IRankingFront?> _mine = new();

        public RankingGroupFront(IPlayerFront player, ITimeFront time)
        {
            _all = new RankingFront[PlayConst.RankingCount];
            for (int i = 0; i < _all.Length; i++)
            {
                var item = _all[i] = new(player);

                item.IsMine.Subscribe(x =>
                {
                    if (x)
                    {
                        _mine.Value = item;
                    }
                    else
                    {
                        if (_mine.Value == item)
                        {
                            _mine.Value = null;
                        }
                    }
                });
            }

            TimeLeftUntilUpdate = time.UtcNow
                .Select(x => 60 - x.Second)
                .ToReadOnlyReactiveProperty();

            _sync = new SyncArrayDestination(_all);
        }

        public IReadOnlyList<IRankingFront> All => _all;

        public ReadOnlyReactiveProperty<IRankingFront?> Mine => _mine;

        public ReadOnlyReactiveProperty<int> TimeLeftUntilUpdate { get; }

        public void Dispose()
        {
            _sync.Dispose();
            _mine.Dispose();
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            reader.ReadDestinationDirtySafely(_sync, mode);
        }
    }
}
