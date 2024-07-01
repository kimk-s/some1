using System;
using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class RegionSectionInfoGroup
    {
        public RegionSectionInfoGroup(IEnumerable<RegionSectionInfo> all)
        {
            ById = all.GroupBy(x => x.Id.Region).ToDictionary(
                x => x.Key,
                x =>
                {
                    var result = x.ToDictionary(x => x.Id.Section)
                        .Values
                        .OrderBy(x => x.Id.Section)
                        .ToArray();

                    if (Enumerable.Range(0, result.Length).Any(x => result[x].Id.Section != x))
                    {
                        throw new InvalidOperationException();
                    }

                    return (IReadOnlyList<RegionSectionInfo>)result;
                });
        }

        public IReadOnlyDictionary<RegionId, IReadOnlyList<RegionSectionInfo>> ById { get; }
    }
}
