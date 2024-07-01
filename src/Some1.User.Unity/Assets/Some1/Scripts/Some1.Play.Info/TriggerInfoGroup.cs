using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class TriggerInfoGroup
    {
        public TriggerInfoGroup(IEnumerable<TriggerInfo> all)
        {
            ById = all.GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    x => (IReadOnlyList<TriggerInfo>)x.ToArray());
        }

        public IReadOnlyDictionary<TriggerId, IReadOnlyList<TriggerInfo>> ById { get; }
    }
}
