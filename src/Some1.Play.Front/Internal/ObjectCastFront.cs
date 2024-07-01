using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class ObjectCastFront : IObjectCastFront, ISyncDestination, ISyncPolatable
    {
        private readonly SyncArrayDestination _sync;
        private readonly NullableUnmanagedParticleDestination<CastPacket> _cast = new();
        private readonly FloatWaveDestination _cycles;

        internal ObjectCastFront(
            ISyncTime syncFrame,
            CharacterCastInfoGroup infos,
            ReadOnlyReactiveProperty<CharacterId?> characterId)
        {
            _sync = new(
                _cast,
                _cycles = new(syncFrame));

            Cycle = characterId
                .CombineLatest(
                    Cast,
                    (characterId, cast) => characterId is null || cast is null ? 0 : infos.ById.GetValueOrDefault(new(characterId.Value, cast.Value.Id))?.Cycle ?? 0)
                .ToReadOnlyReactiveProperty();
        }

        public ReadOnlyReactiveProperty<CastPacket?> Cast => _cast.Value;

        public ReadOnlyReactiveProperty<float> Cycles => _cycles.InterpolatedValue;

        public ReadOnlyReactiveProperty<float> Cycle { get; }

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
