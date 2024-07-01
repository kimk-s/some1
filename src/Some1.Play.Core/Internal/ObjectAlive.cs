using System.Buffers;
using MemoryPack;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;
using R3;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectAlive : IObjectAlive, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly CharacterAliveInfoGroup _infos;
        private readonly ReactiveProperty<bool> _alive = new();
        private readonly ObjectCycles _cycles;
        private readonly ObjectAgent _agent;
        private readonly ObjectIdle _idle;
        private readonly ObjectShift _shift;
        private readonly ObjectCast _cast;
        private readonly ObjectWalk _walk;
        private readonly ObjectBuffGroup _buffs;
        private readonly ObjectBoosterGroup _boosters;
        private readonly ObjectEnergyGroup _energies;
        private readonly ObjectTransfer _transfer;
        private CharacterId? _characterId;

        internal ObjectAlive(
            CharacterAliveInfoGroup infos,
            ObjectAgent agent,
            ObjectIdle idle,
            ObjectShift shift,
            ObjectCast cast,
            ObjectWalk walk,
            ObjectBuffGroup buffs,
            ObjectBoosterGroup boosters,
            ObjectEnergyGroup energies,
            ObjectTransfer transfer,
            ITime time)
        {
            _infos = infos;
            _cycles = new(time, (_, __) => true);
            _agent = agent;
            _idle = idle;
            _shift = shift;
            _cast = cast;
            _walk = walk;
            _buffs = buffs;
            _boosters = boosters;
            _energies = energies;
            _transfer = transfer;
            _sync = new SyncArraySource(
                _alive.ToUnmanagedParticleSource(),
                _cycles);
        }

        public ReadOnlyReactiveProperty<bool> Alive => _alive;

        private void SetAlive(bool value)
        {
            if (Alive.CurrentValue == value)
            {
                return;
            }

            _alive.Value = value;

            UpdateInfo();
            _agent.Stop();
            _idle.Stop();
            _shift.Reset();
            _cast.Stop();
            _walk.Stop();
            _buffs.Stop();
            _boosters.Stop();
            _transfer.IsMoveBlocked = false;
        }

        public IObjectCycles Cycles => _cycles;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal void Set(CharacterId characterId)
        {
            if (_characterId == characterId)
            {
                return;
            }

            _characterId = characterId;

            UpdateInfo();
        }

        internal void Update(float deltaSeconds, ParallelToken parallelToken)
        {
            UpdateAlive();

            _cycles.Update(deltaSeconds, parallelToken);
        }

        internal void UpdateAlive()
        {
            SetAlive(!_energies.All[EnergyId.Health].IsCleared());
        }

        internal void Stop()
        {
            _cycles.Stop();
        }

        internal void Reset()
        {
            SetAlive(false);
            _cycles.Reset();
        }

        private void UpdateInfo()
        {
            var info = _characterId is null
                ? null
                : _infos.ById.GetValueOrDefault(new(_characterId.Value, Alive.CurrentValue));

            _cycles.Cycle = info?.Cycle ?? 0;
            _cycles.Stop(Alive.CurrentValue);
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
