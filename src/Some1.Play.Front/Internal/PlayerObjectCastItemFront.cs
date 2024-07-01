using System;
using System.Linq;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerObjectCastItemFront : IPlayerObjectCastItemFront, ISyncDestination, ISyncPolatable
    {
        private readonly FloatWaveDestination _loadWave;

        internal PlayerObjectCastItemFront(
            ISyncTime syncFrame,
            CastId id,
            CharacterCastInfoGroup infos,
            ReadOnlyReactiveProperty<CharacterId?> characterId)
        {
            _loadWave = new(syncFrame);

            Id = id;

            Info = characterId
                .Select(x => x is null
                    ? null
                    : infos.ById.GetValueOrDefault(new(x.Value, Id)))
                .ToReadOnlyReactiveProperty();

            LoadCount = LoadWave
                .Select(x => (int)MathF.Truncate(x))
                .ToReadOnlyReactiveProperty();

            MaxLoadCount = Info.Select(x => x?.LoadCount ?? 0).ToReadOnlyReactiveProperty();

            AnyLoadCount = LoadCount.Select(x => x > 0).ToReadOnlyReactiveProperty();

            NormalizedLoadWave = LoadWave
                .CombineLatest(
                    MaxLoadCount,
                    (loadWave, maxLoadCount) => maxLoadCount == 0 ? 0 : Math.Clamp(loadWave / maxLoadCount, 0, 1))
                .ToReadOnlyReactiveProperty();

            Delay = NormalizedLoadWave.Select(x => 1 - x).ToReadOnlyReactiveProperty();
        }

        public CastId Id { get; }

        public ReadOnlyReactiveProperty<float> LoadWave => _loadWave.InterpolatedValue;

        public ReadOnlyReactiveProperty<float> NormalizedLoadWave { get; }

        public ReadOnlyReactiveProperty<int> LoadCount { get; }

        public ReadOnlyReactiveProperty<int> MaxLoadCount { get; }

        public ReadOnlyReactiveProperty<bool> AnyLoadCount { get; }

        public ReadOnlyReactiveProperty<float> Delay { get; }

        public ReadOnlyReactiveProperty<CharacterCastInfo?> Info { get; }

        public ReadOnlyReactiveProperty<bool> IsDefault => _loadWave.IsDefault;

        public void Dispose()
        {
            _loadWave.Dispose();
        }

        public void Extrapolate()
        {
            _loadWave.Extrapolate();
        }

        public void Interpolate(float time)
        {
            _loadWave.Interpolate(time);
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            _loadWave.Read(ref reader, mode);
        }

        public void Reset()
        {
            _loadWave.Reset();
        }
    }
}
