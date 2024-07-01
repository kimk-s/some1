using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class CharacterIdleInfoGroup
    {
        public CharacterIdleInfoGroup(IEnumerable<CharacterIdleInfo> all)
        {
            ById = all.ToDictionary(x => x.Id, x => x);
        }

        public IReadOnlyDictionary<CharacterIdleId, CharacterIdleInfo> ById { get; }
    }
}
