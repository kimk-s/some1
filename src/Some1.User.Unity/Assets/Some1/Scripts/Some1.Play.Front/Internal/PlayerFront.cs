using System.Collections.Generic;
using System.Linq;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerFront : IPlayerFront, ISyncDestination, ISyncPolatable
    {
        private readonly SyncArrayDestination _sync;
        private readonly PlayerUnaryFront _unary = new();
        private readonly PlayerGameManagerFront _gameManager;
        private readonly PlayerGameArgsGroupFront _gameArgses = new();
        private readonly PlayerGameFront _game;
        private readonly PlayerGameResultGroupFront _gameResults;
        private readonly PlayerGameToastFront _gameToast;
        private readonly PlayerExpFront _exp;
        private readonly PlayerPremiumFront _premium;
        private readonly PlayerCharacterGroupFront _characters;
        private readonly PlayerSpecialtyGroupFront _specialties;
        private readonly PlayerTitleFront _title;
        private readonly PlayerEmojiGroupFront _emojis;
        private readonly PlayerWelcomeFront _welcome = new();
        private readonly Dictionary<SeasonId, PlayerSeasonFront> _seasons;
        private readonly PlayerObjectFront _object;

        internal PlayerFront(
            ISyncTime syncFrame,
            CharacterInfoGroup characterInfos,
            CharacterAliveInfoGroup characterAliveInfos,
            CharacterCastInfoGroup characterCastInfos,
            CharacterSkillInfoGroup characterSkillInfos,
            CharacterSkillPropInfoGroup characterSkillPropInfos,
            CharacterEnergyInfoGroup characterEnergyInfos,
            CharacterSkinInfoGroup characterSkinInfos,
            CharacterSkinEmojiInfoGroup characterSkinEmojiInfos,
            SpecialtyInfoGroup specialtyInfos,
            SeasonInfoGroup seasonInfos,
            IRegionGroupFront regions,
            ITimeFront time)
        {
            _gameManager = new(syncFrame);
            _game = new(syncFrame);
            _gameResults = new(time);
            _gameToast = new(_gameManager, _gameArgses, _game, _gameResults, time);
            _exp = new(time);
            _premium = new(time);
            _characters = new(
                characterInfos,
                characterSkillInfos,
                characterSkillPropInfos,
                characterEnergyInfos,
                characterSkinInfos,
                _premium,
                time);
            _specialties = new(specialtyInfos, time);
            _title = new(characterInfos.WherePlayerById.Count, specialtyInfos.ById.Count);
            _emojis = new(syncFrame, characterSkinEmojiInfos, _characters);
            _seasons = seasonInfos.ById.Values
                .Select(x => new PlayerSeasonFront(x, Characters, Specialties))
                .ToDictionary(x => x.Id);
            Seasons = _seasons.Values
                .Select(x => (IPlayerSeasonFront)x)
                .ToDictionary(x => x.Id);
            _object = new(
                syncFrame,
                characterAliveInfos,
                characterCastInfos,
                regions);

            _sync = new SyncArrayDestination(
                _unary,
                _gameManager,
                _gameArgses,
                _game,
                _gameResults,
                _exp,
                _premium,
                _characters,
                _specialties,
                _title,
                _emojis,
                _welcome,
                _object);
        }

        public IPlayerUnaryFront Unary => _unary;
        public IPlayerGameManagerFront GameManager => _gameManager;
        public IPlayerGameArgsGroupFront GameArgses => _gameArgses;
        public IPlayerGameFront Game => _game;
        public IPlayerGameResultGroupFront GameResults => _gameResults;
        public IPlayerGameToastFront GameToast => _gameToast;
        public IPlayerExpFront Exp => _exp;
        public IPlayerPremiumFront Premium => _premium;
        public IPlayerCharacterGroupFront Characters => _characters;
        public IPlayerSpecialtyGroupFront Specialties => _specialties;
        public IPlayerTitleFront Title => _title;
        public IPlayerEmojiGroupFront Emojis => _emojis;
        public IPlayerWelcomeFront Welcome => _welcome;
        public IReadOnlyDictionary<SeasonId, IPlayerSeasonFront> Seasons { get; }
        public IPlayerObjectFront Object => _object;

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

        internal void Update(float deltaSeconds)
        {
            _object.Update(deltaSeconds);
        }
    }
}
