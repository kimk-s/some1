using System;
using MemoryPack;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerExpFront : IPlayerExpFront, ISyncDestination
    {
        private readonly SyncArrayDestination _sync;
        private readonly UnmanagedParticleDestination<int> _value = new();
        private readonly UnmanagedParticleDestination<DateTime> _chargeTime = new();

        public PlayerExpFront(ITimeFront time)
        {
            ChargeTimeRemained = ChargeTime.
                CombineLatest(
                    time.UtcNow,
                    (chargeTime, utcNow) => chargeTime == DateTime.MinValue
                        ? TimeSpan.Zero
                        : chargeTime - utcNow)
                .ToReadOnlyReactiveProperty();

            _sync = new SyncArrayDestination(
                _value,
                _chargeTime);
        }

        public ReadOnlyReactiveProperty<int> Value => _value.Value;

        public ReadOnlyReactiveProperty<DateTime> ChargeTime => _chargeTime.Value;

        public ReadOnlyReactiveProperty<TimeSpan> ChargeTimeRemained { get; }

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
