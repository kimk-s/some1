using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;
using Some1.UI;

namespace Some1.User.ViewModel
{
    public sealed class PlayerEmojiGroupViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayerEmojiGroupViewModel(
            IPlayFront front,
            SharedCanExecute sharedCanExecute)
        {
            All = front.Player.Emojis.All.Keys.ToDictionary(
                x => x,
                x => new PlayerEmojiViewModel(x, front, sharedCanExecute).AddTo(_disposables));

            NormalizedLikeDelay = front.Player.Emojis.NormalizedLikeDelay.Connect().AddTo(_disposables);

            LikeUiState = new ReactiveProperty<bool>().AddTo(_disposables);
        }

        public IReadOnlyDictionary<EmojiId, PlayerEmojiViewModel> All { get; }

        public ReadOnlyReactiveProperty<float> NormalizedLikeDelay { get; }

        public ReactiveProperty<bool> LikeUiState { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
