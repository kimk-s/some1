using System;
using System.Buffers;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using MemoryPack;
using R3;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class Object : IObject, IBlockItemValue<ObjectStatic>, ISyncSource
    {
        public const int SelfRootId = 0;
        private static int s_id;
        private readonly bool _leader;
        private readonly SyncArraySource _sync;
        private readonly ObjectTrigger _trigger;
        private readonly ObjectCharacter _character;
        private readonly ObjectBattle _battle;
        private readonly ObjectAlive _alive;
        private readonly ObjectAgent _agent;
        private readonly ObjectIdle _idle;
        private readonly ObjectMessageGroup _messages;
        private readonly ObjectShift _shift;
        private readonly ObjectCast _cast;
        private readonly ObjectWalk _walk;
        private readonly ObjectBuffGroup _buffs;
        private readonly ObjectBoosterGroup _boosters;
        private readonly ObjectSpecialtyGroup _specialties;
        private readonly ObjectTakeStuffGroup _takeStuffs;
        private readonly ObjectGiveStuff _giveStuff;
        private readonly ObjectHitReached _hitReached;
        private readonly ObjectHitGroup _hits;
        private readonly ObjectStatGroup _stats;
        private readonly ObjectEnergyReached _energyReached;
        private readonly ObjectEnergyGroup _energies;
        private readonly ObjectHierarchy _hierarchy;
        private readonly ObjectChildGroup _children;
        private readonly ObjectTransfer _transfer;
        private readonly ObjectTransform _transform;
        private readonly ObjectSkin _skin;
        private readonly ObjectEmoji _emoji;
        private readonly ObjectLike _like;
        private readonly ObjectRegion _region;
        private readonly ObjectTrait _trait;
        private readonly ObjectProperties _properties;
        private readonly ParallelCodeGroup _parallelCodes;
        private readonly Control _control;
        private readonly Space _space;
        private readonly ITime _time;

        internal Object(
            bool leader,
            Object? parent,
            CharacterInfoGroup characterInfos,
            CharacterAliveInfoGroup characterAliveInfos,
            CharacterIdleInfoGroup characterIdleInfos,
            CharacterCastInfoGroup characterCastInfos,
            CharacterCastStatInfoGroup characterCastStatInfos,
            CharacterStatInfoGroup characterStatInfos,
            CharacterEnergyInfoGroup characterEnergyInfos,
            CharacterSkinInfoGroup characterSkinInfos,
            CharacterSkinEmojiInfoGroup characterSkinEmojiInfos,
            BuffInfoGroup buffInfos,
            BuffStatInfoGroup buffStatInfos,
            BoosterInfoGroup boosterInfos,
            SpecialtyInfoGroup _,
            TriggerInfoGroup triggerInfos,
            ParallelOptions parallelOptions,
            IObjectFactory objectFactory,
            RegionGroup regions,
            Space space,
            ITime time)
        {
            Id = Interlocked.Increment(ref s_id);

            _time = time ?? throw new ArgumentNullException(nameof(time));
            _leader = leader;
            _space = space ?? throw new ArgumentNullException(nameof(space));

            _control = new(_time);

            _parallelCodes = new(parallelOptions);

            _properties = new();

            _trait = new();

            _region = new(regions);

            _like = new();
            _like.Added += (_, e) => LikeAdded?.Invoke(this, e);

            _emoji = new(characterSkinEmojiInfos, Id, _properties, _space, _time);
            _emoji.LikeAdded += (_, e) => AddChildBySelf(GetLikeCharacterId(_emoji.Level), e);

            static CharacterId GetLikeCharacterId(byte level) => level switch
            {
                0 => CharacterId.Like0,
                1 => CharacterId.Like1,
                2 => CharacterId.Like2,
                3 => CharacterId.Like3,
                4 => CharacterId.Like4,
                5 => CharacterId.Like5,
                6 => CharacterId.Like6,
                7 => CharacterId.Like7,
                8 => CharacterId.Like8,
                _ => CharacterId.Like0,
            };

            _skin = new(characterSkinInfos, _properties);

            _transform = new(_region, _properties, _time);

            _transfer = new(this, _space);

            _children = new(this, objectFactory, _time);

            _hierarchy = new(this, parent, _children);
            _properties.RootId = _hierarchy.Root.Id;

            _energies = new(characterEnergyInfos);

            _energyReached = new(_energies);

            _stats = new(characterStatInfos, _energies);

            _hits = new(PlayConst.HitCount, _energies, _time);

            _hitReached = new(_hits);

            _giveStuff = new(_properties, _space, () => Alive.Cycles.Value.CurrentValue.B);

            _takeStuffs = new(PlayConst.TakeStuffCount, _time);

            _specialties = new(PlayConst.SpecialtyCount, _takeStuffs);

            _boosters = new(boosterInfos, _takeStuffs, _stats, _properties, _time);

            _buffs = new(
                PlayConst.BuffCount,
                buffInfos,
                buffStatInfos,
                _hits,
                _stats,
                _hierarchy.Root.Id,
                _trait,
                _time);

            _walk = new(_stats, _transfer, _transform, _properties, _space, _time);

            _cast = new(
                Id,
                characterCastInfos,
                characterCastStatInfos,
                _stats,
                _transform,
                _trait,
                _properties,
                _space,
                _time);

            _shift = new(
                _cast,
                _walk,
                _transfer,
                _transform,
                _time);

            _messages = new(
                parallelOptions,
                this,
                _time);

            _idle = new(
                characterIdleInfos,
                _cast,
                _time);

            _agent = new(this, _space, _time);

            _alive = new(
                characterAliveInfos,
                _agent,
                _idle,
                _shift,
                _cast,
                _walk,
                _buffs,
                _boosters,
                _energies,
                _transfer,
                _time);

            _battle = new(
                _alive,
                _idle,
                _shift,
                _cast,
                _buffs,
                _boosters,
                _specialties,
                _takeStuffs,
                _hits,
                _energies,
                _trait);

            _character = new(
                characterInfos,
                _battle,
                _agent,
                _alive,
                _idle,
                _cast,
                _walk,
                _buffs,
                _boosters,
                _specialties,
                _takeStuffs,
                _giveStuff,
                _hitReached,
                _hits,
                _stats,
                _energyReached,
                _energies,
                _skin,
                _like,
                _transfer,
                _properties,
                _time);

            _trigger = new(triggerInfos, this, _space);

            _sync = new SyncArraySource(
                new ReactiveProperty<int>(Id).ToUnmanagedParticleSource(),
                _character,
                _battle,
                _alive,
                _idle,
                _shift,
                _cast,
                _walk,
                _buffs,
                _boosters,
                _takeStuffs,
                _giveStuff,
                _hits,
                _energies,
                _transform,
                _skin,
                _emoji,
                _like,
                _properties);
        }

        internal event EventHandler<ParallelToken>? UpdateBefore;

        internal event EventHandler<Like>? LikeAdded;

        public int Id { get; }

        public bool IsResetReserved { get; private set; }

        public IObjectCharacter Character => _character;

        public IObjectBattle Battle => _battle;

        public IObjectAlive Alive => _alive;

        public IObjectIdle Idle => _idle;

        public IObjectShift Shift => _shift;

        public IObjectCast Cast => _cast;

        public IObjectWalk Walk => _walk;

        public IReadOnlyList<IObjectBuff> Buffs => _buffs.All;

        public IReadOnlyDictionary<BoosterId, IObjectBooster> Boosters => _boosters.All;

        public IReadOnlyList<IObjectSpecialty> Specialties => _specialties.All;

        public IObjectTakeTakeStuffGroup TakeStuffs => _takeStuffs;

        public IObjectGiveStuff GiveStuff => _giveStuff;

        public IObjectHitReached HitReached => _hitReached;

        public IReadOnlyList<IObjectHit> Hits => _hits.All;

        public IReadOnlyDictionary<StatId, IObjectStat> Stats => _stats.All;

        public IObjectEnergyReached EnergyReached => _energyReached;

        public IReadOnlyDictionary<EnergyId, IObjectEnergy> Energies => _energies.All;

        public IObjectTransfer Transfer => _transfer;

        public IObjectTransform Transform => _transform;

        public IObjectSkin Skin => _skin;

        public IObjectEmoji Emoji => _emoji;

        public IObjectLike Like => _like;

        public IObjectRegion Region => _region;

        public IObjectTrait Trait => _trait;

        public IObjectProperties Properties => _properties;

        public IControl Control => _control;

        IObjectHierarchy IObject.Hierarchy => Hierarchy;

        internal ObjectMessageGroup Messages => _messages;

        internal ObjectHierarchy Hierarchy => _hierarchy;

        internal bool IsPlayer => _properties.Player is not null;

        internal Area? Spot { get; set; }

        public BlockIdGroup BlockIds { get; set; }

        public ObjectStatic Static { get; private set; }

        private void UpdateStatic()
        {
            Static = new(Id, _character.Info?.Type ?? CharacterType.Static, _properties.Team);
        }

        internal bool CanSetShift
            => _character.Id.CurrentValue is not null
            && _battle.Battle.CurrentValue == true
            && _alive.Alive.CurrentValue
            && _shift.CanSet;

        internal bool CanSetCast
            => _character.Id.CurrentValue is not null
            && _battle.Battle.CurrentValue == true
            && _alive.Alive.CurrentValue
            && _shift.Shift is null
            && _stats.All[StatId.StunCast].Value < 1;

        internal bool CanSetWalk
            => _character.Id.CurrentValue is not null
            && _battle.Battle.CurrentValue is not null
            && _alive.Alive.CurrentValue
            && _shift.Shift is null
            && _stats.All[StatId.StunWalk].Value < 1;

        internal bool CanAddBuff
            => _character.Id.CurrentValue is not null
            && _battle.Battle.CurrentValue == true
            && _alive.Alive.CurrentValue
            && _shift.Shift?.Id.CanAddHitAndBuff() != false
            && _buffs.CanAdd;

        internal bool CanAddBooster
            => _character.Id.CurrentValue is not null
            && _battle.Battle.CurrentValue == true
            && _alive.Alive.CurrentValue
            && _shift.Shift is null
            && _boosters.CanAdd;

        internal bool CanAddSpecialty
            => _character.Id.CurrentValue is not null
            && _battle.Battle.CurrentValue == true
            && _alive.Alive.CurrentValue
            && _shift.Shift is null
            && _specialties.CanAdd;

        internal bool CanAddHit
            => _character.Id.CurrentValue is not null
            && _battle.Battle.CurrentValue == true
            && _alive.Alive.CurrentValue
            && _shift.Shift?.Id.CanAddHitAndBuff() != false
            && _hits.CanAdd;

        internal bool CanAddEnergy
            => _character.Id.CurrentValue is not null
            && _battle.Battle.CurrentValue == true
            && _alive.Alive.CurrentValue
            && _energies.CanAdd;

        internal bool CanAddChild
            => _character.Id.CurrentValue is not null
            && _battle.Battle.CurrentValue == true
            && _children.CanAdd;

        internal bool CanAddMove
            => _character.Id.CurrentValue is not null
            && _battle.Battle.CurrentValue == true
            && _alive.Alive.CurrentValue
            && _shift.Shift is null
            && !_transfer.IsMoveBlocked;

        internal bool CanAddLike
            => _character.Id.CurrentValue is not null
            && _shift.Shift is null
            && _like.CanAdd;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        public bool Look(Looker looker)
            => Character.Id.CurrentValue is not null
            && Skin.Value
            && Properties.Area.IntersectsWith(looker.Area);

        public bool GetStuffAcquired(int objectId)
        {
            ThrowsIfCharacterIdIsNull();

            if (Character.Info?.Type != CharacterType.Stuff)
            {
                return false;
            }

            return _trigger.GetStuffAcquired(objectId);
        }

        internal bool TryTakeControl()
        {
            return _control.TryTake();
        }

        internal bool Update(bool leader, ParallelToken parallelToken)
        {
            if (_leader != leader || !_control.TryTake())
            {
                return false;
            }

            UpdateInternal(parallelToken);
            return true;
        }

        private void UpdateInternal(ParallelToken parallelToken)
        {
            if (_character.Id.CurrentValue is null)
            {
                return;
            }

            _sync.ClearDirty();

            UpdateBefore?.Invoke(this, parallelToken);

            if (IsResetReserved)
            {
                Reset(parallelToken);
                return;
            }

            _alive.Update(_time.DeltaSeconds, parallelToken);
            _agent.Update(_time.DeltaSeconds, parallelToken);
            if (!_alive.Alive.CurrentValue)
            {
                _messages.Update(parallelToken);
                _takeStuffs.Update(_time.DeltaSeconds);
                _hits.Update(_time.DeltaSeconds);
                _children.Update(parallelToken);
                _transfer.Update(parallelToken);
                _emoji.Update(_time.DeltaSeconds, parallelToken);
            }
            else
            {
                _idle.Update(_time.DeltaSeconds, parallelToken);
                _messages.Update(parallelToken);
                _shift.Update(_time.DeltaSeconds, parallelToken);
                _cast.Update(_time.DeltaSeconds, parallelToken);
                _walk.Update(_time.DeltaSeconds, parallelToken);
                _buffs.Update(_time.DeltaSeconds, parallelToken);
                _boosters.Update(_time.DeltaSeconds);
                _takeStuffs.Update(_time.DeltaSeconds);
                _giveStuff.Update(parallelToken);
                _hitReached.Update(parallelToken);
                _hits.Update(_time.DeltaSeconds);
                _energyReached.Update(_time.DeltaSeconds, parallelToken);
                _children.Update(parallelToken);
                _transfer.Update(parallelToken);
                _emoji.Update(_time.DeltaSeconds, parallelToken);
                _trait.Update();
            }
        }

        internal bool SetCharacter(CharacterId value)
        {
            IsResetReserved = false;

            _character.Set(value);

            UpdateStatic();
            return true;
        }

        internal void SetBattle(bool value)
        {
            _battle.SetBattle(value);
        }

        internal void SetEnergiesValueRate(float rate)
        {
            _energies.SetValueRate(rate);
        }

        internal void SetAgentAim(Aim aim)
        {
            ThrowsIfCharacterIdIsNull();

            _agent.Set(aim);
        }

        internal bool SetShift(ShiftId id, float rotation, float distance, float time, float speed)
        {
            if (!CanSetShift)
            {
                return false;
            }

            return _shift.Set(id, rotation, distance, time, speed);
        }

        internal bool SetCast(Cast? cast)
        {
            if (!CanSetCast)
            {
                return false;
            }

            _cast.Set(cast);
            return true;
        }

        internal bool TrySetCastScan(CastId id, ParallelToken parallelToken)
        {
            if (!CanSetCast)
            {
                return false;
            }

            if (Character.Info?.Type.IsUnit() != true)
            {
                throw new InvalidOperationException();
            }

            return _cast.TrySetScan(id, parallelToken);
        }

        internal bool CanConsumeCast(CastId id)
        {
            return _cast.CanConsume(id);
        }

        internal bool SetWalk(Walk? walk)
        {
            if (!CanSetWalk)
            {
                return false;
            }

            _walk.Set(walk);
            return true;
        }

        internal bool TrySetWalkScan(float length, ObjectTarget objectTarget, ParallelToken parallelToken)
        {
            if (!CanSetWalk)
            {
                return false;
            }

            if (Character.Info?.Type.IsUnit() != true)
            {
                throw new InvalidOperationException();
            }

            return _walk.TrySetScan(length, objectTarget, parallelToken);
        }

        internal bool SetEmoji(EmojiId emoji)
        {
            ThrowsIfCharacterIdIsNull();
            
            return _emoji.Set(emoji);
        }

        internal void SetEmojiLevel(byte value)
        {
            ThrowsIfCharacterIdIsNull();

            _emoji.SetLevel(value);
        }

        internal void SetTransfer(Vector2 position, ParallelToken? parallelToken)
        {
            ThrowsIfCharacterIdIsNull();

            _transfer.Set(position, parallelToken);
            _shift.Stop();
            _cast.Stop();
            _walk.Stop();
        }

        internal void WarpTransfer(Vector2 position, ParallelToken? parallelToken, bool addWarpOutEffect = true)
        {
            ThrowsIfCharacterIdIsNull();

            if (addWarpOutEffect &&
                _transform.Position.CurrentValue.B != Vector2.Zero &&
                _transform.Position.CurrentValue.B != position)
            {
                AddChildBySelf(GetWarpOutId(_properties.Team), parallelToken);
            }
            
            _transfer.Warp(position, parallelToken);
            _shift.Stop();
            _cast.Stop();
            _walk.Stop();

            AddChildBySelf(GetWarpInId(_properties.Team), parallelToken);

            static CharacterId GetWarpOutId(byte team) => team == Team.Player ? CharacterId.WarpOut1 : CharacterId.WarpOut2;
            static CharacterId GetWarpInId(byte team) => team == Team.Player ? CharacterId.WarpIn1 : CharacterId.WarpIn2;
        }

        internal void SetTransformPositionBySpace(Vector2Wave position)
        {
            _transform.SetPosition(position);
        }

        internal void SetTransformRotation(float value)
        {
            ThrowsIfCharacterIdIsNull();

            _transform.SetInstanceRotation(value);
        }

        internal void SetAim(Aim aim)
        {
            ThrowsIfCharacterIdIsNull();

            _transform.SetInstanceRotation(aim.Rotation);
            _properties.Aim = aim;
        }

        internal void SetSkin(SkinId? value)
        {
            ThrowsIfCharacterIdIsNull();

            _skin.Set(value);
        }

        internal void SetTeamProperty(byte team)
        {
            ThrowsIfCharacterIdIsNull();

            _properties.SetTeam(team);

            UpdateStatic();
        }

        internal void SetPlayerProperty(ObjectPlayer? player)
        {
            ThrowsIfCharacterIdIsNull();

            _properties.Player = player;
        }

        internal void SetAnchorProperty(Vector2? anchor)
        {
            ThrowsIfCharacterIdIsNull();

            _properties.Anchor = anchor;
        }

        internal void SetBirthIdProperty(BirthId? birthid)
        {
            ThrowsIfCharacterIdIsNull();

            _properties.BirthId = birthid;
        }

        internal void PinSpecialty(SpecialtyId id, bool value)
        {
            _specialties.Pin(id, value);
        }

        internal bool AddBuffBySelf(BuffId id)
        {
            return _buffs.Add(id, _hierarchy.Root.Id, 0, _properties.SkinId ?? SkinId.Skin0, _stats.All[StatId.Offense].Value, _trait.Value, _properties.Team);
        }

        internal bool AddBuff(BuffId id, int rootId, float angle, SkinId skinId, int offenseStat, Trait traitId, byte team)
        {
            if (!CanAddBuff)
            {
                return false;
            }

            return _buffs.Add(id, rootId, angle, skinId, offenseStat, traitId, team);
        }

        internal void StopBuff(BuffId id)
        {
            _buffs.Stop(id);
        }

        internal bool AddBooster(BoosterId id, int number, bool ignoreCheckCanAdd = false)
        {
            if (!ignoreCheckCanAdd && !CanAddBooster)
            {
                return false;
            }

            return _boosters.Add(id, number, ignoreCheckCanAdd);
        }

        internal bool AddSpecialty(SpecialtyId id, int number)
        {
            if (!CanAddSpecialty)
            {
                return false;
            }

            return _specialties.Add(id, number);
        }

        internal bool AddHit(HitId id, int value, int rootId, float angle, BirthId? birthId)
        {
            if (!CanAddHit)
            {
                return false;
            }

            return _hits.Add(id, value, rootId, angle, birthId);
        }

        internal bool AddEnergy(EnergyId id, int value)
        {
            if (!CanAddEnergy)
            {
                return false;
            }

            return _energies.Add(id, value);
        }

        private bool AddChildBySelf(CharacterId characterId, ParallelToken? parallelToken)
        {
            return _children.Add(
                characterId,
                _properties.SkinId,
                _properties.Team,
                _properties.Power,
                _properties.Area.Position,
                Aim.Zero,
                parallelToken);
        }

        internal bool AddChild(
            CharacterId characterId,
            SkinId? skinId,
            byte team,
            int power,
            Vector2 position,
            Aim aim,
            ParallelToken? parallelToken)
        {
            if (!CanAddChild)
            {
                return false;
            }

            return _children.Add(
                characterId,
                skinId,
                team,
                power,
                position,
                aim,
                parallelToken);
        }

        internal bool AddMove(Vector2 velocity)
        {
            if (!CanAddMove)
            {
                return false;
            }

            _transfer.MoveDelta += MoveHelper.CalculateDistance(velocity, _time.DeltaSeconds);
            return true;
        }

        internal bool AddLike(Like like)
        {
            if (!CanAddLike)
            {
                return false;
            }

            return _like.Add(like);
        }

        internal bool IsSpaceObject(Area area, ObjectTarget target, ParallelToken parallelToken)
            => _parallelCodes.TrySet(parallelToken)
            && IntersectsWith(area)
            && IsTarget(target);

        internal bool IntersectsWith(Area area) => _properties.Area.IntersectsWith(area);

        internal bool IsTarget(ObjectTarget target) => Static.IsTarget(target);

        internal void ResetOrReserve(ParallelToken? parallelToken)
        {
            if (!_control.TryTake())
            {
                IsResetReserved = true;
                return;
            }

            Reset(parallelToken);
        }

        internal void Reset(ParallelToken? parallelToken)
        {
            IsResetReserved = false;

            if (_character.Id.CurrentValue is null)
            {
                return;
            }

            _trigger.Reset();
            _character.Reset();
            _battle.Reset();
            _alive.Reset();
            _agent.Reset();
            _idle.Reset();
            _messages.Reset(true);
            _shift.Reset();
            _cast.Reset();
            _walk.Reset();
            _buffs.Reset();
            _boosters.Reset();
            _specialties.Reset();
            _takeStuffs.Reset();
            _giveStuff.Reset();
            _hitReached.Reset();
            _hits.Reset();
            _stats.Reset();
            _energyReached.Reset();
            _energies.Reset();
            _hierarchy.Reset();
            _children.Reset();
            _transfer.Reset(parallelToken);
            _transform.Reset();
            _skin.Reset();
            _emoji.Reset();
            _like.Reset();
            _region.Reset();
            _trait.Reset();
            _properties.Reset();

            Static = new();
        }

        private void ThrowsIfCharacterIdIsNull()
        {
            if (_character.Id.CurrentValue is null)
            {
                throw new InvalidOperationException();
            }
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
