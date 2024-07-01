using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class BuffInfoGroup
    {
        public BuffInfoGroup(IEnumerable<BuffInfo> all)
        {
            ById = all.ToDictionary(x => x.Id, x => x);
        }

        public IReadOnlyDictionary<BuffId, BuffInfo> ById { get; }
    }
}
