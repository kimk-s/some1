using System.Collections.Generic;
using System.Linq;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class ObjectFront : IObjectFront, ISyncDestination, ISyncPolatable
    {
        private readonly SyncArrayDestination _sync;
        private readonly UnmanagedParticleDestination<int> _id = new();
        private readonly ObjectCharacterFront _character;
        private readonly ObjectBattleFront _battle;
        private readonly ObjectAliveFront _alive;
        private readonly ObjectIdleFront _idle;
        private readonly ObjectShiftFront _shift;
        private readonly ObjectCastFront _cast;
        private readonly ObjectWalkFront _walk;
        private readonly ObjectBuffFront[] _buffs;
        private readonly Dictionary<BoosterId, ObjectBoosterFront> _boosters;
        private readonly ObjectTakeStuffGroupFront _takeStuffs;
        private readonly ObjectGiveStuffFront _giveStuff;
        private readonly ObjectHitGroupFront _hits;
        private readonly Dictionary<EnergyId, ObjectEnergyFront> _energies;
        private readonly ObjectTransformFront _transform;
        private readonly NullableUnmanagedParticleDestination<SkinId> _skinId = new();
        private readonly ObjectEmojiFront _emoji;
        private readonly ObjectLikeGroupFront _likes;
        private readonly ObjectPropertiesFront _properties;

        internal ObjectFront(
            ISyncTime syncFrame,
            CharacterInfoGroup characterInfos,
            CharacterAliveInfoGroup characterAliveInfos,
            CharacterIdleInfoGroup characterIdleInfos,
            CharacterCastInfoGroup charcterCastInfos,
            CharacterSkinInfoGroup characterSkinInfos,
            CharacterSkinEmojiInfoGroup characterSkinEmojiInfos,
            BuffSkinInfoGroup buffSkinInfos,
            BoosterInfoGroup boosterInfos,
            IPlayerObjectFront playerObject)
        {
            Active = Id.Select(x => x > 0).ToReadOnlyReactiveProperty();
            _character = new(characterInfos);
            _battle = new();
            _alive = new(syncFrame, characterAliveInfos, Character.Id);
            _idle = new(syncFrame, characterIdleInfos, Character.Id);
            _shift = new(syncFrame, Character.Info.Select(x => x?.Shift).ToReadOnlyReactiveProperty());
            _cast = new(syncFrame, charcterCastInfos, Character.Id);
            _walk = new(syncFrame, Character.Info.Select(x => x?.Walk).ToReadOnlyReactiveProperty());
            _buffs = Enumerable.Range(0, PlayConst.BuffCount)
                .Select(_ => new ObjectBuffFront(syncFrame, buffSkinInfos))
                .ToArray();
            _boosters = boosterInfos.ById.Values
                .Select(x => new ObjectBoosterFront(x, syncFrame))
                .ToDictionary(x => x.Id);
            Boosters = _boosters.Values
                .Select(x => (IObjectBoosterFront)x)
                .ToDictionary(x => x.Id);
            _takeStuffs = new(syncFrame);
            _giveStuff = new();
            _hits = new(syncFrame, Id, playerObject);
            _energies = EnumForUnity.GetValues<EnergyId>()
                .Select(x => new ObjectEnergyFront(x))
                .ToDictionary(x => x.Id);
            Energies = _energies.Values
                .Select(x => (IObjectEnergyFront)x)
                .ToDictionary(x => x.Id);
            _transform = new(syncFrame, Id);
            SkinId = _skinId.Value
                .CombineLatest(
                    Character.Id,
                    (skinId, characterId) => skinId is null || characterId is null
                        ? null
                        : characterSkinInfos.ById.ContainsKey(new(characterId.Value, skinId.Value))
                            ? skinId
                            : skinId == Info.SkinId.Skin0
                                ? null
                                : characterSkinInfos.ById.ContainsKey(new(characterId.Value, Info.SkinId.Skin0))
                                    ? Info.SkinId.Skin0
                                    : null)
                .ToReadOnlyReactiveProperty();
            _emoji = new(syncFrame, characterSkinEmojiInfos, Character, SkinId);
            _likes = new(Id);
            _properties = new();

            Relation = Id.CombineLatest(Properties.Team, playerObject.Id, playerObject.Team, (id, team, id2, team2) =>
            {
                if (id == id2)
                {
                    return ObjectRelation.Mine;
                }
                if (new TeamTarget(TeamTargetInfo.Ally, team).IsMatch(team2))
                {
                    return ObjectRelation.Ally;
                }
                if (new TeamTarget(TeamTargetInfo.Enemy, team).IsMatch(team2))
                {
                    return ObjectRelation.Enemy;
                }
                return (ObjectRelation?)null;
            }).ToReadOnlyReactiveProperty();

            _sync = new(
                _id,
                _character,
                _battle,
                _alive,
                _idle,
                _shift,
                _cast,
                _walk,
                new SyncArrayDestination(_buffs),
                new SyncArrayDestination(_boosters.Values.OrderBy(x => x.Id)),
                _takeStuffs,
                _giveStuff,
                _hits,
                new SyncArrayDestination(_energies.Values.OrderBy(x => x.Id)),
                _transform,
                _skinId,
                _emoji,
                _likes,
                _properties);
        }

        public ReadOnlyReactiveProperty<int> Id => _id.Value;
        public ReadOnlyReactiveProperty<bool> Active { get; }
        public IObjectCharacterFront Character => _character;
        public IObjectBattleFront Battle => _battle;
        public IObjectAliveFront Alive => _alive;
        public IObjectIdleFront Idle => _idle;
        public IObjectShiftFront Shift => _shift;
        public IObjectCastFront Cast => _cast;
        public IObjectWalkFront Walk => _walk;
        public IReadOnlyList<IObjectBuffFront> Buffs => _buffs;
        public IReadOnlyDictionary<BoosterId, IObjectBoosterFront> Boosters { get; }
        public IObjectTakeStuffGroupFront TakeStuffs => _takeStuffs;
        public IObjectGiveStuffFront GiveStuff => _giveStuff;
        public IObjectHitGroupFront Hits => _hits;
        public IReadOnlyDictionary<EnergyId, IObjectEnergyFront> Energies { get; }
        public IObjectTransformFront Transform => _transform;
        public ReadOnlyReactiveProperty<SkinId?> SkinId { get; }
        public IObjectEmojiFront Emoji => _emoji;
        public IReadOnlyList<IObjectLikeFront> Likes => _likes.All;
        public IObjectPropertiesFront Properties => _properties;
        public ReadOnlyReactiveProperty<ObjectRelation?> Relation { get; }

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
            _transform.Update(deltaSeconds);
            _likes.Update(deltaSeconds);
        }
    }
}
