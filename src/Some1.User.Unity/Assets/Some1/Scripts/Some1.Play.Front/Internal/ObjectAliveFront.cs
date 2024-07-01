using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class ObjectAliveFront : IObjectAliveFront, ISyncDestination, ISyncPolatable
    {
        private readonly SyncArrayDestination _sync;
        private readonly UnmanagedParticleDestination<bool> _alive = new();
        private readonly FloatWaveDestination _cycles;

        internal ObjectAliveFront(
            ISyncTime syncFrame,
            CharacterAliveInfoGroup infos,
            ReadOnlyReactiveProperty<CharacterId?> characterId)
        {
            _sync = new(
                _alive,
                _cycles = new(syncFrame));

            Cycle = characterId
                .CombineLatest(
                    Alive,
                    (characterId, alive) => characterId is null ? 0 : infos.ById.GetValueOrDefault(new(characterId.Value, alive))?.Cycle ?? 0)
                .ToReadOnlyReactiveProperty();
        }

        public ReadOnlyReactiveProperty<bool> Alive => _alive.Value;

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
