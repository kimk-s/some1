using System;
using System.Buffers;
using MemoryPack;
using R3;
using Some1.Play.Data;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class PlayerTitle : IPlayerTitle, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly ReactiveProperty<int> _star = new();
        private readonly ReactiveProperty<PlayerId> _playerId = new();
        private readonly ReactiveProperty<int> _likePoint = new();
        private readonly ReactiveProperty<Medal> _medal = new();
        private readonly ReadOnlyReactiveProperty<Leveling> _starGrade;
        private readonly ReadOnlyReactiveProperty<Leveling> _like;
        private readonly ReadOnlyReactiveProperty<Title> _title;

        internal PlayerTitle(Object @object, int playerCharacterCount, int specialtyCount)
        {
            int maxStar = playerCharacterCount * PlayConst.CharacterStarLeveling_MaxLevel + specialtyCount * PlayConst.SpecialtyStarLeveling_MaxLevel;

            _starGrade = _star.Select(x => new Leveling(x, 1, maxStar, LevelingMethod.Plain))
                .ToReadOnlyReactiveProperty();

            _like = _likePoint.Select(x => new Leveling(x, PlayConst.LikeLeveling_MaxLevel, PlayConst.LikeLeveling_StepFactor, LevelingMethod.Pow))
               .ToReadOnlyReactiveProperty();

            _like.Skip(1).Subscribe(x => @object.SetEmojiLevel(checked((byte)x.Level)));

            _title = _star
                .CombineLatest(
                    _playerId,
                    _like,
                    _medal,
                    (star, playerId, like, medal) => new Title(star, playerId, checked((byte)like.Level), medal))
                .ToReadOnlyReactiveProperty();

            _title.Skip(1).Subscribe(x => @object.SetPlayerProperty(x == Title.Empty ? null : (ObjectPlayer?)new ObjectPlayer(x)));

            _sync = new SyncArraySource(
                _star.ToUnmanagedParticleSource(),
                _playerId.ToUnmanagedParticleSource(),
                _likePoint.ToUnmanagedParticleSource(),
                _medal.ToUnmanagedParticleSource());
        }

        public Leveling StarGrade => _starGrade.CurrentValue;

        public PlayerId PlayerId => _playerId.Value;

        public Leveling Like => _like.CurrentValue;

        public Medal Medal => _medal.Value;

        public Title Title => _title.CurrentValue;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        internal void Load(TitleData? data)
        {
            if (data is null)
            {
                return;
            }

            _likePoint.Value = data.Value.LikePoint;
        }

        internal TitleData Save()
        {
            return new(_likePoint.Value);
        }

        internal void SetPlayerId(PlayerId playerId)
        {
            _playerId.Value = playerId;
        }

        internal void AddStar(int star)
        {
            if (star < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(star));
            }

            _star.Value += star;
        }

        internal void AddLikePoint(Like like)
        {
            _likePoint.Value++;
        }

        internal void SetMedal(Medal medal)
        {
            _medal.Value = medal;
        }

        internal void Reset()
        {
            _star.Value = 0;
            _playerId.Value = PlayerId.Empty;
            _likePoint.Value = 0;
            _medal.Value = Medal.None;
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
