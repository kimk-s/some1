using System;
using System.Buffers;
using MemoryPack;
using R3;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectCharacter : IObjectCharacter, ISyncSource
    {
        private readonly NullableUnmanagedParticleSource<CharacterId> _sync;
        private readonly ReactiveProperty<CharacterId?> _id = new();
        private readonly CharacterInfoGroup _infos;
        private readonly ObjectBattle _battle;
        private readonly ObjectAgent _agent;
        private readonly ObjectAlive _alive;
        private readonly ObjectIdle _idle;
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
        private readonly ObjectEnergyReached _energyReachaed;
        private readonly ObjectEnergyGroup _energies;
        private readonly ObjectSkin _skin;
        private readonly ObjectLike _like;
        private readonly ObjectTransfer _transfer;
        private readonly ObjectProperties _properties;
        private readonly ITime _time;

        public ObjectCharacter(
            CharacterInfoGroup infos,
            ObjectBattle battle,
            ObjectAgent agent,
            ObjectAlive alive,
            ObjectIdle idle,
            ObjectCast cast,
            ObjectWalk walk,
            ObjectBuffGroup buffs,
            ObjectBoosterGroup boosters,
            ObjectSpecialtyGroup specialties,
            ObjectTakeStuffGroup takeStuffs,
            ObjectGiveStuff giveStuff,
            ObjectHitReached hitReached,
            ObjectHitGroup hits,
            ObjectStatGroup stats,
            ObjectEnergyReached energyReachaed,
            ObjectEnergyGroup energies,
            ObjectSkin skin,
            ObjectLike like,
            ObjectTransfer transfer,
            ObjectProperties properties,
            ITime time)
        {
            _infos = infos;
            _battle = battle;
            _agent = agent;
            _alive = alive;
            _idle = idle;
            _cast = cast;
            _walk = walk;
            _buffs = buffs;
            _boosters = boosters;
            _specialties = specialties;
            _takeStuffs = takeStuffs;
            _giveStuff = giveStuff;
            _hitReached = hitReached;
            _hits = hits;
            _stats = stats;
            _energyReachaed = energyReachaed;
            _energies = energies;
            _skin = skin;
            _like = like;
            _transfer = transfer;
            _properties = properties;
            _time = time;
            _sync = _id.ToNullableUnmanagedParticleSource();
        }

        public ReadOnlyReactiveProperty<CharacterId?> Id => _id;

        public CharacterInfo? Info { get; private set; }

        internal CharacterId? LastIdForDebug { get; private set; }

        internal int LastIdFrameCountForDebug { get; private set; }

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal void Set(CharacterId value)
        {
            if (Id.CurrentValue == value)
            {
                return;
            }

            var info = _infos.ById[value];
            if (Info is not null && Info.Type != info.Type)
            {
                throw new InvalidOperationException("Invalid Character Type.");
            }
            Info = info;
            _id.Value = info.Id;

            _battle.Set(Info);
            _agent.Set(Info.Agent);
            _alive.Set(value);
            _idle.Set(value);
            _cast.Set(value);
            _walk.Set(Info.Walk, Info.Move);
            _buffs.Enabled = Info.IsBuffEnabled;
            _boosters.Enabled = Info.IsBoosterEnabled;
            _specialties.Enabled = Info.IsSpecialtyEnabled;
            _takeStuffs.Enabled = Info.IsTakeStuffEnabled;
            _giveStuff.Set(Info.GiveStuff);
            _hitReached.Set(Info.HitReached);
            _hits.Enabled = Info.IsHitEnabled;
            _stats.Set(value);
            _energyReachaed.Set(Info.EnergyReached);
            _energies.Set(value);
            _skin.Set(value);
            _like.Enabled = Info.IsLikeEnabled;
            _transfer.Set(Info.Move, Info.Walk);
            _properties.Set(Info);

            if (_energies.All[EnergyId.Health].MaxValue.CurrentValue < 1)
            {
                throw new InvalidOperationException("Invalid Character Energy.");
            }
        }

        internal void Reset()
        {
            if (Id.CurrentValue is not null)
            {
                LastIdForDebug = Id.CurrentValue;
                LastIdFrameCountForDebug = _time.FrameCount;
            }

            Info = null;
            _id.Value = null;
            _properties.Set(null);
        }

        internal CharacterAreaInfo GetAreaInfo() => Info?.Area ?? throw new InvalidOperationException();

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
