using System.Buffers;
using MemoryPack;
using R3;
using Some1.Play.Data;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class PlayerEmoji : IPlayerEmoji, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly ReactiveProperty<EmojiId?> _emoji = new();
        private readonly ReactiveProperty<FloatWave> _delay = new();
        private readonly ReactiveProperty<FloatWave> _likeDelay = new();
        private readonly Object _object;

        public PlayerEmoji(Object @object, ITime time)
        {
            _object = @object;
            _sync = new SyncArraySource(
                _emoji.ToNullableUnmanagedParticleSource(),
                _delay.ToWaveSource(time),
                _likeDelay.ToWaveSource(time));
        }

        public EmojiId? Emoji { get => _emoji.Value; private set => _emoji.Value = value; }

        public float Delay => _delay.Value.B;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        public float LikeDelay => _likeDelay.Value.B;

        internal void Load(EmojiData? data)
        {
            if (data is null)
            {
                return;
            }

            _likeDelay.Value = new(data.Value.LikeDelay);
        }

        internal EmojiData Save()
        {
            return new(LikeDelay);
        }

        internal void Set(EmojiId emoji)
        {
            if (Emoji is not null)
            {
                return;
            }

            if (emoji.IsLike() && LikeDelay > 0)
            {
                return;
            }

            if (!EnumForUnity.IsDefined(emoji))
            {
                return;
            }

            Emoji = emoji;

            _delay.Value = new(PlayConst.PlayerEmojiDelay);

            if (emoji.IsLike())
            {
                _likeDelay.Value = new(PlayConst.PlayerEmojiLikeDelay);
            }

            _object.SetEmoji(emoji);
        }

        internal void Update(float deltaSeconds)
        {
            if (LikeDelay > 0)
            {
                _likeDelay.Value = _likeDelay.Value.Flow(-deltaSeconds);

                if (LikeDelay <= 0)
                {
                    ResetLikeDelay();
                }
            }

            if (Delay > 0)
            {
                _delay.Value = _delay.Value.Flow(-deltaSeconds);

                if (Delay <= 0)
                {
                    ResetEmoji();
                }
            }
        }

        internal void Reset()
        {
            ResetEmoji();
            ResetLikeDelay();
        }

        private void ResetEmoji()
        {
            Emoji = null;
            _delay.Value = FloatWave.Zero;
        }

        private void ResetLikeDelay()
        {
            _likeDelay.Value = FloatWave.Zero;
        }

        public void ClearDirty()
        {
            _sync.ClearDirty();
        }

        public void Write<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, SyncMode mode) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
        {
            _sync.Write(ref writer, mode);
        }

        public void Dispose()
        {
            _sync.Dispose();
        }
    }
}
