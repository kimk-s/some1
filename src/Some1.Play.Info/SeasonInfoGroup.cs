using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class SeasonInfoGroup
    {
        public SeasonInfoGroup(IEnumerable<SeasonInfo> all)
        {
            ById = all.ToDictionary(x => x.Id, x => x);
            Current = ById.Values.Single(x => x.Type == SeasonType.Current);
        }

        public IReadOnlyDictionary<SeasonId, SeasonInfo> ById { get; }

        public SeasonInfo Current { get; }
    }
}
