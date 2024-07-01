using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class SpecialtyInfoGroup
    {
        public SpecialtyInfoGroup(IEnumerable<SpecialtyInfo> all)
        {
            ById = all.ToDictionary(x => x.Id);
        }

        public IReadOnlyDictionary<SpecialtyId, SpecialtyInfo> ById { get; }
    }
}
