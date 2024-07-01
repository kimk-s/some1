using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class ObjectHitFront : IObjectHitFront, ISyncDestination, ISyncPolatable
    {
        private readonly SyncArrayDestination _sync;
        private readonly NullableUnmanagedParticleDestination<HitPacket> _packet = new();
        private readonly FloatWaveDestination _cycles;

        internal ObjectHitFront(ISyncTime syncFrame, ReadOnlyReactiveProperty<int> objectId, IPlayerObjectFront playerObject)
        {
            Hit = _packet.Value;
            
            ToMe = objectId
                .CombineLatest(playerObject.Id, (a, b) => a != 0 && a == b)
                .ToReadOnlyReactiveProperty();

            FromMe = Hit
                .CombineLatest(
                    playerObject.Id,
                    (a, b) => a is not null && a.Value.RootId != 0 && a.Value.RootId == b)
                .ToReadOnlyReactiveProperty();

            _sync = new(
                _packet,
                _cycles = new(syncFrame));
        }

        public ReadOnlyReactiveProperty<HitPacket?> Hit { get; }

        public ReadOnlyReactiveProperty<float> Cycles => _cycles.InterpolatedValue;

        public ReadOnlyReactiveProperty<bool> ToMe { get; }

        public ReadOnlyReactiveProperty<bool> FromMe { get; }

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        public void Dispose()
        {
            _sync.Dispose();
        }

        public void Extrapolate()
        {
            _sync.Extrapolate();
        }

        public void Interpolate(float time)
        {
            _sync.Interpolate(time);
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
