using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;
using System;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerSpecialtyFront : IPlayerSpecialtyFront, ISyncDestination
    {
        private readonly SyncArrayDestination _sync;
        private readonly UnmanagedParticleDestination<int> _number = new();
        private readonly UnmanagedParticleDestination<DateTime> _numberUtc = new();

        public PlayerSpecialtyFront(SpecialtyInfo info, ITimeFront time)
        {
            Id = info.Id;
            Season = info.Season;
            Region = info.Region;
            Star = Number
                .Select(x => new Leveling(x, PlayConst.SpecialtyStarLeveling_MaxLevel, PlayConst.SpecialtyStarLeveling_StepFactor, LevelingMethod.Plain))
                .ToReadOnlyReactiveProperty();
            NumberTimeAgo = _numberUtc.Value
                .CombineLatest(
                    time.UtcNow,
                    (numberUtc, utcNow) => numberUtc == DateTime.MinValue ? (TimeSpan?)null : (utcNow - numberUtc))
                .ToReadOnlyReactiveProperty();

            _sync = new SyncArrayDestination(
                _number,
                _numberUtc);
        }

        public SpecialtyId Id { get; }

        public SeasonId Season { get; }

        public RegionId Region { get; }

        public ReadOnlyReactiveProperty<int> Number => _number.Value;

        public ReadOnlyReactiveProperty<Leveling> Star { get; }

        public ReadOnlyReactiveProperty<TimeSpan?> NumberTimeAgo { get; }

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
