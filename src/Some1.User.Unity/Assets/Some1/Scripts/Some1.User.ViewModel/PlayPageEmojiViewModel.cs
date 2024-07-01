using System;
using R3;
using Some1.Play.Front;

namespace Some1.User.ViewModel
{
    public sealed class PlayPageEmojiViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayPageEmojiViewModel(IPlayerEmojiGroupFront front)
        {
            NormalizedDelay = front.NormalizedDelay.Connect().AddTo(_disposables);
        }

        public ReadOnlyReactiveProperty<float> NormalizedDelay { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
