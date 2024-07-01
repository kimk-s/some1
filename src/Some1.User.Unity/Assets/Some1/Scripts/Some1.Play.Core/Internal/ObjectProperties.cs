using System.Buffers;
using System.Numerics;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;
using R3;
using System;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectProperties : IObjectProperties, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly ReactiveProperty<byte> _team = new();
        private readonly ReactiveProperty<ObjectPlayer?> _player = new();

        public ObjectProperties()
        {
            _sync = new SyncArraySource(
                _team.ToUnmanagedParticleSource(),
                _player.ToNullableUnmanagedParticleSource());
        }

        public Area Area { get; private set; }

        public CharacterType? CharacterType { get; private set; }

        public ObjectPlayer? Player { get => _player.Value; internal set => _player.Value = value; }

        public int Power { get; internal set; }

        public int RootId { get; internal set; }

        public Aim Aim { get; internal set; }

        public SkinId? SkinId { get; internal set; }

        ReadOnlyReactiveProperty<byte> IObjectProperties.Team => _team;

        internal byte Team => _team.Value;

        public Vector2? Anchor { get; internal set; }

        public BirthId? BirthId { get; internal set; }

        internal float Rotation
        {
            get => Aim.Rotation;
            set => Aim = new(value, 0);
        }

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal void Set(CharacterInfo? info)
        {
            CharacterType = info?.Type;
            SetAreaInfo(info?.Area);
        }

        internal void SetAreaPosition(Vector2 value)
        {
            var area = Area;
            area.Position = value;
            Area = area;
        }

        internal void SetTeam(byte value)
        {
            if (_team.Value == value)
            {
                return;
            }

            if (_team.Value != Info.Team.Neutral)
            {
                throw new InvalidOperationException();
            }

            _team.Value = value;
        }

        internal void Reset()
        {
            Area = default;
            CharacterType = null;
            Player = null;
            Power = 0;
            // RootId : no reset
            Aim = default;
            SkinId = null;
            _team.Value = Info.Team.Neutral;
            Anchor = null;
            BirthId = null;
        }

        private void SetAreaInfo(CharacterAreaInfo? value)
        {
            var area = Area;
            area.Type = value?.Type ?? default;
            area.Size = value is null ? default : new(value.Size, value.Size);
            Area = area;
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
