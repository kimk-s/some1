using System;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;
using Some1.UI;

namespace Some1.User.ViewModel
{
    public sealed class PlayerEmojiViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayerEmojiViewModel(
            EmojiId id,
            IPlayFront front,
            SharedCanExecute sharedCanExecute)
        {
            var emoji = front.Player.Emojis.All[id];

            Emoji = emoji.Emoji;

            Level = front.Player.Title.Like.Select(x => checked((byte)x.Level)).ToReadOnlyReactiveProperty();

            Set = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);
            Set.SubscribeAwait(
                sharedCanExecute,
                async (x, ct) => await front.SetEmojiAsync(emoji.Id, ct),
                AwaitOperation.Drop);
        }

        public ReadOnlyReactiveProperty<CharacterSkinEmojiId?> Emoji { get; }

        public ReadOnlyReactiveProperty<byte> Level { get; }

        public ReactiveCommand<Unit> Set { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
