using System;
using MemoryPack;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerPremiumFront : IPlayerPremiumFront, ISyncDestination
    {
        private readonly UnmanagedParticleDestination<DateTime> _endTime = new();

        internal PlayerPremiumFront(ITimeFront time)
        {
            IsPremium = EndTime
                .CombineLatest(time.UtcNow, (endTime, utcNow) => endTime > utcNow)
                .ToReadOnlyReactiveProperty();
            TimeLeft = EndTime
                .CombineLatest(time.UtcNow, (endTime, utcNow) => endTime > utcNow ? endTime - utcNow : TimeSpan.Zero)
                .ToReadOnlyReactiveProperty();
        }

        public ReadOnlyReactiveProperty<DateTime> EndTime => _endTime.Value;

        public ReadOnlyReactiveProperty<bool> IsPremium { get; }

        public ReadOnlyReactiveProperty<TimeSpan> TimeLeft { get; }

        public ReadOnlyReactiveProperty<bool> IsDefault => _endTime.IsDefault;

        public void Dispose()
        {
            _endTime.Dispose();
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            _endTime.Read(ref reader, mode);
        }

        public void Reset()
        {
            _endTime.Reset();
        }
    }
}
