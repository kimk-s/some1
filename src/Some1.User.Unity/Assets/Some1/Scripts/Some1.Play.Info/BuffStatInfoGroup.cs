using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class BuffStatInfoGroup
    {
        public BuffStatInfoGroup(IEnumerable<BuffStatInfo> all)
        {
            ById = all.GroupBy(x => x.Id).ToDictionary(
                x => x.Key,
                x => (IReadOnlyList<BuffStatInfo>)x.ToList());
        }

        public IReadOnlyDictionary<BuffId, IReadOnlyList<BuffStatInfo>> ById { get; }
    }
}
