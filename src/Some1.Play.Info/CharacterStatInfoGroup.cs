using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class CharacterStatInfoGroup
    {
        public CharacterStatInfoGroup(IEnumerable<CharacterStatInfo> all)
        {
            ById = all.ToDictionary(x => x.Id, x => x);

            GroupByCharacterId = all.GroupBy(x => x.Id.Character)
                .ToDictionary(
                    x => x.Key,
                    x => (IReadOnlyList<CharacterStatInfo>)x.ToArray());
        }

        public IReadOnlyDictionary<CharacterStatId, CharacterStatInfo> ById { get; }

        public IReadOnlyDictionary<CharacterId, IReadOnlyList<CharacterStatInfo>> GroupByCharacterId { get; }
    }
}
