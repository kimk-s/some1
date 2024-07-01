using System;
using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class CharacterSkinInfoGroup
    {
        public CharacterSkinInfoGroup(IEnumerable<CharacterSkinInfo> all)
        {
            if (all is null)
            {
                throw new ArgumentNullException(nameof(all));
            }

            ById = all.ToDictionary(x => x.Id, x => x);
            GroupByCharacterId = all.GroupBy(x => x.Id.Character)
                .ToDictionary(
                    x => x.Key,
                    x => (IReadOnlyDictionary<SkinId, CharacterSkinInfo>)x.ToDictionary(x => x.Id.Skin, x => x));
        }

        public IReadOnlyDictionary<CharacterSkinId, CharacterSkinInfo> ById { get; }

        public IReadOnlyDictionary<CharacterId, IReadOnlyDictionary<SkinId, CharacterSkinInfo>> GroupByCharacterId { get; }
    }
}
