using System.Collections.Generic;
using System.Linq;
using MemoryPack;
using R3;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerGameArgsGroupFront : IPlayerGameArgsGroupFront, ISyncDestination
    {
        private readonly SyncArrayDestination _sync;
        private readonly Dictionary<GameMode, PlayerGameArgsFront> _all;

        internal PlayerGameArgsGroupFront()
        {
            _all = EnumForUnity.GetValues<GameMode>()
                .Select(x => new PlayerGameArgsFront(x))
                .ToDictionary(x => x.Id);
            All = _all.Values
                .Select(x => (IPlayerGameArgsFront)x)
                .ToDictionary(x => x.Id);
            Selected = Observable.Merge(All.Values.Select(x => x.IsSelected.Where(x => x).Select(_ => x)))!
                .ToReadOnlyReactiveProperty()!;

            _sync = new SyncArrayDestination(_all.Values.OrderBy(x => x.Id));
        }

        public IReadOnlyDictionary<GameMode, IPlayerGameArgsFront> All { get; }

        public ReadOnlyReactiveProperty<IPlayerGameArgsFront?> Selected { get; }

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
