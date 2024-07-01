using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;
using R3;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectEnergyGroup : ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly Dictionary<EnergyId, ObjectEnergy> _all;
        private CharacterId? _characterId;

        internal ObjectEnergyGroup(CharacterEnergyInfoGroup infos)
        {
            _all = EnumForUnity.GetValues<EnergyId>().ToDictionary(x => x, x => new ObjectEnergy(x, infos));
            All = _all.ToDictionary(
                x => x.Key,
                x => (IObjectEnergy)x.Value);
            _sync = new SyncArraySource(_all.Values.OrderBy(x => x.Id));
        }

        internal IReadOnlyDictionary<EnergyId, IObjectEnergy> All { get; }

        internal bool CanAdd => true;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal void Set(CharacterId characterId)
        {
            if (_characterId == characterId)
            {
                return;
            }

            _characterId = characterId;

            foreach (var item in _all.Values)
            {
                item.Set(characterId);
            }
        }

        internal void SetStatValue(EnergyId id, int value)
        {
            _all[id].SetStatValue(value);
        }

        internal bool Add(EnergyId id, int value)
        {
            if (!CanAdd)
            {
                return false;
            }

            _all[id].SetValue(_all[id].Value.CurrentValue + value);

            return true;
        }

        internal void SetValueRate(float rate = 1.0f)
        {
            foreach (var item in _all.Values)
            {
                item.SetValueRate(rate);
            }
        }

        internal void Clear(EnergyId id)
        {
            _all[id].Clear();
        }

        internal void Consume(EnergyId id, int cost)
        {
            _all[id].Consume(cost);
        }

        internal void Reset()
        {
            _characterId = null;
            foreach (var item in _all.Values)
            {
                item.Reset();
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
