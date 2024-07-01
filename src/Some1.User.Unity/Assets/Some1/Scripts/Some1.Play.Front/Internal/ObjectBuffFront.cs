using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class ObjectBuffFront : IObjectBuffFront, ISyncDestination, ISyncPolatable
    {
        private readonly SyncArrayDestination _sync;
        private readonly NullableUnmanagedParticleDestination<BuffPacket> _packet = new();
        private readonly FloatWaveDestination _cycles;

        internal ObjectBuffFront(ISyncTime syncFrame, BuffSkinInfoGroup skinInfos)
        {
            Id = _packet.Value
                .Select(x => x is null
                    ? (BuffSkinId?)null
                    : skinInfos.ById.ContainsKey(new(x.Value.Id, x.Value.SkinId))
                        ? new BuffSkinId(x.Value.Id, x.Value.SkinId)
                        : x.Value.SkinId == SkinId.Skin0
                            ? null
                            : skinInfos.ById.ContainsKey(new(x.Value.Id, SkinId.Skin0))
                                ? new BuffSkinId(x.Value.Id, SkinId.Skin0)
                                : null)
                .ToReadOnlyReactiveProperty();

            _sync = new(
                _packet,
                _cycles = new(syncFrame));
        }

        public ReadOnlyReactiveProperty<BuffSkinId?> Id { get; }

        public ReadOnlyReactiveProperty<float> Cycles => _cycles.InterpolatedValue;

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
