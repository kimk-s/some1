using System.Collections.Generic;
using System.Linq;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerGameResultGroupFront : IPlayerGameResultGroupFront, ISyncDestination
    {
        private readonly SyncArrayDestination _sync;
        private readonly PlayerGameResultFront[] _all;

        public PlayerGameResultGroupFront(ITimeFront time)
        {
            _all = Enumerable.Range(0, PlayConst.PlayerGameResultCount)
                .Select(x => new PlayerGameResultFront(time))
                .ToArray();

            Latest = Observable
                .Merge(All
                    .Select(x => x.Result
                        .Where(x => Latest?.CurrentValue?.Result.CurrentValue is null || (x is not null && x.Value.EndTime > Latest.CurrentValue.Result.CurrentValue.Value.EndTime))
                        .Select(_ => (IPlayerGameResultFront?)x))
                    .Append(new ReactiveProperty<IPlayerGameResultFront?>()))
                .ToReadOnlyReactiveProperty();

            _sync = new SyncArrayDestination(_all);
        }

        public IReadOnlyList<IPlayerGameResultFront> All => _all;

        public ReadOnlyReactiveProperty<IPlayerGameResultFront?> Latest { get; }

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        public void Dispose()
        {
            _sync.Dispose();
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
