using System;
using System.Buffers;
using MemoryPack;
using R3;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectBattle : IObjectBattle, ISyncSource
    {
        private readonly NullableUnmanagedParticleSource<bool> _sync;
        private readonly ReactiveProperty<bool?> _battle = new();
        private readonly ObjectAlive _alive;
        private readonly ObjectIdle _idle;
        private readonly ObjectShift _shift;
        private readonly ObjectCast _cast;
        private readonly ObjectBuffGroup _buffs;
        private readonly ObjectBoosterGroup _boosters;
        private readonly ObjectSpecialtyGroup _specialties;
        private readonly ObjectTakeStuffGroup _takeStuffs;
        private readonly ObjectHitGroup _hits;
        private readonly ObjectEnergyGroup _energies;
        private readonly ObjectTrait _trait;
        private CharacterInfo? _characterInfo;

        internal ObjectBattle(
            ObjectAlive alive,
            ObjectIdle idle,
            ObjectShift shift,
            ObjectCast cast,
            ObjectBuffGroup buffs,
            ObjectBoosterGroup boosters,
            ObjectSpecialtyGroup specialties,
            ObjectTakeStuffGroup takeStuffs,
            ObjectHitGroup hits,
            ObjectEnergyGroup energies,
            ObjectTrait trait)
        {
            _alive = alive;
            _idle = idle;
            _shift = shift;
            _cast = cast;
            _buffs = buffs;
            _boosters = boosters;
            _specialties = specialties;
            _takeStuffs = takeStuffs;
            _hits = hits;
            _energies = energies;
            _trait = trait;
            _sync = _battle.ToNullableUnmanagedParticleSource();
        }

        public ReadOnlyReactiveProperty<bool?> Battle => _battle;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal void Set(CharacterInfo characterInfo)
        {
            _characterInfo = characterInfo;
        }

        internal void SetBattle(bool value)
        {
            if (_characterInfo is null)
            {
                throw new InvalidOperationException();
            }

            if (Battle.CurrentValue == value)
            {
                return;
            }

            _battle.Value = value;

            _alive.Stop();
            _idle.Stop();
            _shift.Stop();
            _cast.SetBattle(value);
            _buffs.Stop();
            _boosters.Stop();
            _specialties.Stop();
            _takeStuffs.Stop();
            _hits.Stop();
            _energies.SetValueRate();

            if (_characterInfo.IsPlayer)
            {
                _cast.ClearCharge();
            }
            else
            {
                _cast.FillCharge();
            }

            if (value)
            {
                _trait.Start();
            }
            else
            {
                _trait.Reset();
            }

            _alive.UpdateAlive();
        }

        internal void Reset()
        {
            _characterInfo = null;
            _battle.Value = null;
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
