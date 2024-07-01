using System;
using Some1.Play.Front;
using Some1.Play.Info;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class ObjectUnitEmojiViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public ObjectUnitEmojiViewModel(IObjectEmojiFront front)
        {
            Emoji = front.Emoji.Connect().AddTo(_disposables);
            Level = front.Level.Connect().AddTo(_disposables);
            Cycles = front.Cycles.Connect().AddTo(_disposables);
        }

        public ReadOnlyReactiveProperty<CharacterSkinEmojiId?> Emoji { get; }

        public ReadOnlyReactiveProperty<byte> Level { get; }

        public ReadOnlyReactiveProperty<float> Cycles { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
