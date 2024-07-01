using System;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerGameResultFront : IPlayerGameResultFront, ISyncDestination
    {
        private readonly NullablePackableParticleDestination<GameResult> _result = new();

        internal PlayerGameResultFront(ITimeFront time)
        {
            EndTimeAgo = Result
                .CombineLatest(
                    time.UtcNow,
                    (result, utcNow) => result is null
                        ? TimeSpan.Zero
                        : utcNow - result.Value.EndTime)
                .ToReadOnlyReactiveProperty();
        }

        public ReadOnlyReactiveProperty<GameResult?> Result => _result.Value;

        public ReadOnlyReactiveProperty<TimeSpan> EndTimeAgo { get; }

        public ReadOnlyReactiveProperty<bool> IsDefault => _result.IsDefault;

        public void Dispose()
        {
            _result.Dispose();
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            _result.Read(ref reader, mode);
        }

        public void Reset()
        {
            _result.Reset();
        }
    }
}
