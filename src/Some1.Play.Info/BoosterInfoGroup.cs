using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class BoosterInfoGroup
    {
        public BoosterInfoGroup(IEnumerable<BoosterInfo> all)
        {
            ById = all.ToDictionary(x => x.Id, x => x);
        }

        public IReadOnlyDictionary<BoosterId, BoosterInfo> ById { get; }
    }
}
