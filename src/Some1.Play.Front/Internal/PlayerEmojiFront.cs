using System;
using System.Linq;
using Some1.Play.Info;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerEmojiFront : IPlayerEmojiFront
    {
        internal PlayerEmojiFront(
            EmojiId id,
            CharacterSkinEmojiInfoGroup infos,
            IPlayerCharacterGroupFront characters)
        {
            Id = id;
            var character = characters.Selected.Select(x => x?.Id);
            var skin = new ReactiveProperty<SkinId?>();
            IDisposable? electedSkinSubscription = null;
            characters.Selected.Subscribe(x =>
            {
                electedSkinSubscription?.Dispose();
                electedSkinSubscription = x?.ElectedSkin.Subscribe(x => skin.Value = x?.Id.Skin);
            });
            Emoji = character
                .CombineLatest(skin, (c, s) => c is null || s is null ? null : infos.Get(c.Value, s.Value, Id)?.Id)
                .ToReadOnlyReactiveProperty();
        }

        public EmojiId Id { get; }

        public ReadOnlyReactiveProperty<CharacterSkinEmojiId?> Emoji { get; }
    }
}
