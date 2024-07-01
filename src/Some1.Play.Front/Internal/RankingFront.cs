using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class RankingFront : IRankingFront, ISyncDestination
    {
        private readonly PackableParticleDestination<Ranking> _ranking = new();

        public RankingFront(IPlayerFront player)
        {
            IsMine = _ranking.Value.Select(x => x.Title.PlayerId)
                .CombineLatest(player.Title.PlayerId, (a, b) => a == b)
                .ToReadOnlyReactiveProperty();
        }

        public ReadOnlyReactiveProperty<Ranking> Ranking => _ranking.Value;

        public ReadOnlyReactiveProperty<bool> IsMine { get; }

        public ReadOnlyReactiveProperty<bool> IsDefault => _ranking.IsDefault;

        public void Dispose()
        {
            _ranking.Dispose();
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            _ranking.Read(ref reader, mode);
        }

        public void Reset()
        {
            _ranking.Reset();
        }
    }
}
