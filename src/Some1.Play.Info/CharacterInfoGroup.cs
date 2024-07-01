using System;
using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class CharacterInfoGroup
    {
        public CharacterInfoGroup(IEnumerable<CharacterInfo> all)
        {
            if (all is null)
            {
                throw new ArgumentNullException(nameof(all));
            }

            ById = all.ToDictionary(x => x.Id);

            WherePlayerById = all.Where(x => x.IsPlayer)
                .ToDictionary(x => x.Id);

            WherePlayerBySeason = all.Where(x => x.IsPlayer && x.Season is not null)
                .GroupBy(x => x.Season)
                .ToDictionary(
                    x => x.Key!.Value,
                    x => (IReadOnlyList<CharacterInfo>)x.ToArray());
        }

        public IReadOnlyDictionary<CharacterId, CharacterInfo> ById { get; }

        public IReadOnlyDictionary<CharacterId, CharacterInfo> WherePlayerById { get; }

        public IReadOnlyDictionary<SeasonId, IReadOnlyList<CharacterInfo>> WherePlayerBySeason { get; }
    }
}
