using MemoryPack;
using R3;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;

namespace Some1.Play.Front.Internal
{
    internal sealed class ObjectEmojiFront : IObjectEmojiFront, ISyncDestination, ISyncPolatable
    {
        private readonly SyncArrayDestination _sync;
        private readonly NullableUnmanagedParticleDestination<EmojiId> _emoji = new();
        private readonly UnmanagedParticleDestination<byte> _level = new();
        private readonly FloatWaveDestination _cycles;

        internal ObjectEmojiFront(
            ISyncTime syncFrame,
            CharacterSkinEmojiInfoGroup infos,
            IObjectCharacterFront character,
            ReadOnlyReactiveProperty<SkinId?> skin)
        {
            Emoji = character.Id
                .CombineLatest(skin, _emoji.Value, (c, s, e) =>
                {
                    return c is null || s is null || e is null
                        ? null
                        : infos.Get(c.Value, s.Value, e.Value)?.Id;
                })
                .ToReadOnlyReactiveProperty();

            _sync = new(
                _emoji,
                _level,
                _cycles = new(syncFrame));
        }

        public ReadOnlyReactiveProperty<CharacterSkinEmojiId?> Emoji { get; }

        public ReadOnlyReactiveProperty<byte> Level => _level.Value;

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
