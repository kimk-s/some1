using System.Collections.Generic;
using System.Linq;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectStatGroup
    {
        private readonly Dictionary<StatId, ObjectStat> _all;

        internal ObjectStatGroup(CharacterStatInfoGroup infos, ObjectEnergyGroup energies)
        {
            _all = EnumForUnity.GetValues<StatId>().ToDictionary(x => x, x => new ObjectStat(x, infos, energies));
            All = _all.ToDictionary(
                x => x.Key,
                x => (IObjectStat)x.Value);
        }

        internal IReadOnlyDictionary<StatId, IObjectStat> All { get; }

        internal void Set(CharacterId characterId)
        {
            foreach (var item in _all.Values)
            {
                item.Set(characterId);
            }
        }

        internal void SetCastValue(StatId id, int value)
        {
            _all[id].SetCastValue(value);
        }

        internal void SetBuffValue(StatId id, int value, int index)
        {
            _all[id].SetBuffValue(value, index);
        }

        internal void SetBoosterValue(StatId id, int value)
        {
            _all[id].SetBoosterValue(value);
        }

        internal void ResetCastValue()
        {
            foreach (var item in _all.Values)
            {
                item.SetCastValue(0);
            }
        }

        internal void ResetBuffValue(int index)
        {
            foreach (var item in _all.Values)
            {
                item.SetBuffValue(0, index);
            }
        }

        internal void Reset()
        {
            foreach (var item in _all.Values)
            {
                item.Reset();
            }
        }
    }
}
