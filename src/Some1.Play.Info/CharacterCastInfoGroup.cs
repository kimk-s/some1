using System;
using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class CharacterCastInfoGroup
    {
        public CharacterCastInfoGroup(IEnumerable<CharacterCastInfo> all)
        {
            if (all is null)
            {
                throw new ArgumentNullException(nameof(all));
            }

            ById = all.ToDictionary(x => x.Id);

            GroupByCharacterId = all.GroupBy(x => x.Id.Character)
                .ToDictionary(
                    x => x.Key,
                    x => (IReadOnlyDictionary<CastId, CharacterCastInfo>)x.ToDictionary(x => x.Id.Cast));
        }

        public IReadOnlyDictionary<CharacterCastId, CharacterCastInfo> ById { get; }

        public IReadOnlyDictionary<CharacterId, IReadOnlyDictionary<CastId, CharacterCastInfo>> GroupByCharacterId { get; }
    }
}
