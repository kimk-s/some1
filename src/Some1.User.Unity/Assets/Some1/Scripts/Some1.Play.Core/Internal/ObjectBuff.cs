using System.Buffers;
using MemoryPack;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;
using R3;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectBuff : IObjectBuff, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly ReactiveProperty<BuffId?> _id = new();
        private readonly ReactiveProperty<SkinId?> _skinId = new();
        private readonly BuffInfoGroup _infos;
        private readonly BuffStatInfoGroup _statInfos;
        private readonly ObjectHitGroup _hits;
        private readonly ObjectStatGroup _stats;
        private readonly ObjectTrait _trait;
        private readonly ObjectCycles _cycles;

        internal ObjectBuff(
            int index,
            BuffInfoGroup infos,
            BuffStatInfoGroup statInfos,
            ObjectHitGroup hits,
            ObjectStatGroup stats,
            ObjectTrait trait,
            ITime time)
        {
            Index = index;
            _infos = infos;
            _statInfos = statInfos;
            _hits = hits;
            _stats = stats;
            _trait = trait;
            _cycles = new(time, (x, _) => x < 1);
            _sync = new SyncArraySource(
                _id.CombineLatest(_skinId, (id, skinId) => id is null || skinId is null ? (BuffPacket?)null : new BuffPacket(id.Value, skinId.Value))
                    .ToReadOnlyReactiveProperty().ToNullableUnmanagedParticleSource(),
                _cycles);
        }

        public int Index { get; }

        public BuffId? Id { get => _id.Value; private set => _id.Value = value; }

        public int RootId { get; private set; }

        public SkinId? SkinId { get => _skinId.Value; private set => _skinId.Value = value; }

        public int OffenseStat { get; private set; }

        public Trait Trait { get; private set; }

        public byte Team { get; private set; }

        public int Token { get; private set; }

        public IObjectCycles Cycles => _cycles;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal void Set(BuffId id, int rootId, float angle, SkinId skinId, int offenceStat, Trait traitId, byte team, int token)
        {
            var info = _infos.ById[id];
            var statInfos = _statInfos.ById.GetValueOrDefault(id);

            _cycles.Cycle = info.Cycle;
            _cycles.Stop();

            _trait.SetBuffValue(Index, ((info.Trait?.Condition ?? 0) & _trait.Next.CurrentValue) != 0);

            _stats.ResetBuffValue(Index);
            if (statInfos is not null)
            {
                for (int i = 0; i < statInfos.Count; i++)
                {
                    var item = statInfos[i];
                    if ((item.Condition & traitId) != 0)
                    {
                        _stats.SetBuffValue(item.StatId, item.StatValue, Index);
                    }
                }
            }

            if (info.Hit is not null && (info.Hit.Condition & traitId) != 0)
            {
                _hits.Add(
                    info.Hit.HitId,
                    StatHelper.CalculateDamage(info.Hit.BaseValue, offenceStat, _stats.All[StatId.Defense].Value),
                    rootId,
                    angle,
                    null);
            }

            Id = id;
            RootId = rootId;
            SkinId = skinId;
            OffenseStat = offenceStat;
            Trait = traitId;
            Team = team;
            Token = token;
        }

        internal void Update(float deltaSeconds, ParallelToken parallelToken)
        {
            float remainDeltaSeconds = _cycles.Update(deltaSeconds, parallelToken);
            if (remainDeltaSeconds > 0)
            {
                Reset();
            }
        }

        internal void Reset()
        {
            if (Id is null)
            {
                return;
            }

            _cycles.Reset();

            _trait.SetBuffValue(Index, false);

            _stats.ResetBuffValue(Index);

            Id = null;
            RootId = 0;
            SkinId = null;
            OffenseStat = 0;
            Trait = default;
            Team = default;
            Token = 0;
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
