using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class ObjectIdleFront : IObjectIdleFront, ISyncDestination, ISyncPolatable
    {
        private readonly SyncArrayDestination _sync;
        private readonly UnmanagedParticleDestination<bool> _idle = new();
        private readonly FloatWaveDestination _cycles;

        internal ObjectIdleFront(
            ISyncTime syncFrame,
            CharacterIdleInfoGroup infos,
            ReadOnlyReactiveProperty<CharacterId?> characterId)
        {
            _sync = new(
                _idle,
                _cycles = new(syncFrame));

            Cycle = characterId
                .CombineLatest(
                    Idle,
                    (characterId, idle) => characterId is null ? 0 : infos.ById.GetValueOrDefault(new(characterId.Value, idle))?.Cycle ?? 0)
                .ToReadOnlyReactiveProperty();
        }

        public ReadOnlyReactiveProperty<bool> Idle => _idle.Value;

        public ReadOnlyReactiveProperty<float> Cycles => _cycles.InterpolatedValue;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        public ReadOnlyReactiveProperty<float> Cycle { get; }

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
