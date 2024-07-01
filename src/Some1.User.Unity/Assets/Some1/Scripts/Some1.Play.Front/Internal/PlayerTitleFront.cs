using System.Linq;
using MemoryPack;
using R3;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerTitleFront : IPlayerTitleFront, ISyncDestination
    {
        private readonly SyncArrayDestination _sync;
        private readonly UnmanagedParticleDestination<int> _star = new();
        private readonly UnmanagedParticleDestination<PlayerId> _playerId = new();
        private readonly UnmanagedParticleDestination<int> _likePoint = new();
        private readonly UnmanagedParticleDestination<Medal> _medal = new();

        public PlayerTitleFront(int playerCharacterCount, int specialtyCount)
        {
            MaxStar = playerCharacterCount * PlayConst.CharacterStarLeveling_MaxLevel + specialtyCount * PlayConst.SpecialtyStarLeveling_MaxLevel;

            StarGrade = _star.Value.Select(x => new Leveling(x, 1, MaxStar, LevelingMethod.Plain))
                .ToReadOnlyReactiveProperty();

            Like = _likePoint.Value.Select(x => new Leveling(x, PlayConst.LikeLeveling_MaxLevel, PlayConst.LikeLeveling_StepFactor, LevelingMethod.Pow))
                .ToReadOnlyReactiveProperty();

            _sync = new(
                _star,
                _playerId,
                _likePoint,
                _medal);
        }

        public int MaxStar { get; }

        public ReadOnlyReactiveProperty<Leveling> StarGrade { get; }

        public ReadOnlyReactiveProperty<PlayerId> PlayerId => _playerId.Value;

        public ReadOnlyReactiveProperty<Leveling> Like { get; }

        public ReadOnlyReactiveProperty<Medal> Medal => _medal.Value;

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
