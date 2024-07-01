using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class TalkInfoGroup
    {
        public TalkInfoGroup(IEnumerable<TalkInfo> all)
        {
            ById = all.ToDictionary(x => x.Id);
        }

        public IReadOnlyDictionary<TalkId, TalkInfo> ById { get; }
    }
}
