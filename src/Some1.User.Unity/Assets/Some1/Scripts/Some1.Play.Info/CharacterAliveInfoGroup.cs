using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class CharacterAliveInfoGroup
    {
        public CharacterAliveInfoGroup(IEnumerable<CharacterAliveInfo> all)
        {
            ById = all.ToDictionary(x => x.Id, x => x);
        }

        public IReadOnlyDictionary<CharacterAliveId, CharacterAliveInfo> ById { get; }
    }
}
