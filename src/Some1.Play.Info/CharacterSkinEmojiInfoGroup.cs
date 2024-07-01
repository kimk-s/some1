using System;
using System.Collections.Generic;
using System.Linq;

namespace Some1.Play.Info
{
    public sealed class CharacterSkinEmojiInfoGroup
    {
        public CharacterSkinEmojiInfoGroup(IEnumerable<CharacterSkinEmojiInfo> all)
        {
            if (all is null)
            {
                throw new ArgumentNullException(nameof(all));
            }

            ById = all.ToDictionary(x => x.Id, x => x);
        }

        public IReadOnlyDictionary<CharacterSkinEmojiId, CharacterSkinEmojiInfo> ById { get; }

        public CharacterSkinEmojiInfo? Get(CharacterId characterId, SkinId skinId, EmojiId emojiId)
        {
            if (ById.TryGetValue(new(characterId, skinId, emojiId), out var result))
            {
                return result;
            }

            if (ById.TryGetValue(new(null, skinId, emojiId), out result))
            {
                return result;
            }

            if (ById.TryGetValue(new(null, null, emojiId), out result))
            {
                return result;
            }

            return null;
        }
    }
}
