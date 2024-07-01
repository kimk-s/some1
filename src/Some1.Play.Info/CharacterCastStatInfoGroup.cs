using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class CharacterCastStatInfoGroup
    {
        public CharacterCastStatInfoGroup(IEnumerable<CharacterCastStatInfo> all)
        {
            ById = all.GroupBy(x => x.Id).ToDictionary(
                x => x.Key,
                x => (IReadOnlyList<CharacterCastStatInfo>)x.ToList());
        }

        public IReadOnlyDictionary<CharacterCastId, IReadOnlyList<CharacterCastStatInfo>> ById { get; }
    }
}
