using System.Collections.Generic;
using System.Linq;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerEmojiGroupFront : IPlayerEmojiGroupFront, ISyncDestination, ISyncPolatable
    {
        private readonly SyncArrayDestination _sync;
        private readonly NullableUnmanagedParticleDestination<EmojiId> _emoji = new();
        private readonly FloatWaveDestination _delay;
        private readonly FloatWaveDestination _likeDelay;

        internal PlayerEmojiGroupFront(
            ISyncTime syncFrame,
            CharacterSkinEmojiInfoGroup infos,
            IPlayerCharacterGroupFront characters)
        {
            _delay = new(syncFrame);
            _likeDelay = new(syncFrame);

            All = EnumForUnity.GetValues<EmojiId>()
                .Select(x => new PlayerEmojiFront(x, infos, characters))
                .Select(x => (IPlayerEmojiFront)x)
                .ToDictionary(x => x.Id);

            NormalizedDelay = Delay.Select(x => x / PlayConst.PlayerEmojiDelay).ToReadOnlyReactiveProperty();
            NormalizedLikeDelay = LikeDelay.Select(x => x / PlayConst.PlayerEmojiLikeDelay).ToReadOnlyReactiveProperty();

            _sync = new SyncArrayDestination(
                _emoji,
                _delay,
                _likeDelay);
        }

        public IReadOnlyDictionary<EmojiId, IPlayerEmojiFront> All { get; }

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        public ReadOnlyReactiveProperty<float> Delay => _delay.InterpolatedValue;

        public ReadOnlyReactiveProperty<float> LikeDelay => _likeDelay.InterpolatedValue;

        public ReadOnlyReactiveProperty<float> NormalizedDelay { get; }

        public ReadOnlyReactiveProperty<float> NormalizedLikeDelay { get; }

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
