using System;
using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class CharacterEnergyInfoGroup
    {
        public CharacterEnergyInfoGroup(IEnumerable<CharacterEnergyInfo> all)
        {
            if (all is null)
            {
                throw new ArgumentNullException(nameof(all));
            }

            ById = all.ToDictionary(x => x.Id, x => x);

            GroupByCharacterId = all.GroupBy(x => x.Id.Character)
                .ToDictionary(
                x => x.Key,
                x => (IReadOnlyDictionary<EnergyId, CharacterEnergyInfo>)x.ToDictionary(
                    y => y.Id.Energy,
                    y => y));
        }

        public IReadOnlyDictionary<CharacterEnergyId, CharacterEnergyInfo> ById { get; }

        public IReadOnlyDictionary<CharacterId, IReadOnlyDictionary<EnergyId, CharacterEnergyInfo>> GroupByCharacterId { get; }
    }
}
