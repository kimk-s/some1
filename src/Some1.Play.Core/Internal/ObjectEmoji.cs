using System;
using System.Buffers;
using MemoryPack;
using R3;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectEmoji : IObjectEmoji, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly CharacterSkinEmojiInfoGroup _infos;
        private readonly int _objectId;
        private readonly ReactiveProperty<EmojiId?> _emoji = new();
        private readonly ReactiveProperty<byte> _level = new();
        private readonly ReactiveProperty<FloatWave> _cycles = new();
        private readonly IObjectProperties _properties;
        private readonly Space _space;

        internal ObjectEmoji(CharacterSkinEmojiInfoGroup infos, int objectId, IObjectProperties properties, Space space, ITime time)
        {
            _infos = infos;
            _objectId = objectId;
            _properties = properties;
            _space = space;
            _sync = new SyncArraySource(
                _emoji.ToNullableUnmanagedParticleSource(),
                _level.ToUnmanagedParticleSource(),
                _cycles.ToWaveSource(time));
        }

        public EmojiId? Emoji => _emoji.Value;

        public byte Level => _level.Value;

        public float Cycles => _cycles.Value.B;

        internal bool CanSet => Emoji is null;

        internal event EventHandler<ParallelToken>? LikeAdded;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal bool Set(EmojiId emoji)
        {
            if (!CanSet)
            {
                return false;
            }

            if (!EnumForUnity.IsDefined(emoji))
            {
                return false;
            }

            _emoji.Value = emoji;

            return true;
        }

        internal void SetLevel(byte value)
        {
            _level.Value = value;
        }

        internal void Update(float deltaSeconds, ParallelToken parallelToken)
        {
            if (Emoji is null)
            {
                return;
            }

            if (Cycles == default && Emoji.Value.IsLike() && _properties.Player is not null)
            {
                _space.AddLike(
                    new(_properties.Player.Value.Title.PlayerId.EnsureNotEmpty()),
                    _objectId,
                    new(TeamTargetInfo.Ally, _properties.Team.CurrentValue),
                    Area.Circle(_properties.Area.Position, PlayConst.ObjectEmojiLikeCircleDiameter),
                    parallelToken);

                LikeAdded?.Invoke(this, parallelToken);
            }

            _cycles.Value = _cycles.Value.Flow(deltaSeconds / PlayConst.ObjectEmojiCycle);
            if (_cycles.Value.B > 1)
            {
                Reset();
            }
        }

        internal void Reset()
        {
            _emoji.Value = null;
            _level.Value = 0;
            _cycles.Value = default;
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
