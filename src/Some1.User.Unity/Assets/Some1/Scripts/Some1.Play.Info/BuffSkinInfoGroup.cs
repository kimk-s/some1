using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class BuffSkinInfoGroup
    {
        public BuffSkinInfoGroup(IEnumerable<BuffSkinInfo> all)
        {
            ById = all.ToDictionary(x => x.Id);
        }

        public IReadOnlyDictionary<BuffSkinId, BuffSkinInfo> ById { get; }
    }
}
