using System;
using R3;
using Some1.Prefs.Front;

namespace Some1.Prefs.UI
{
    public sealed class ThemeViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public ThemeViewModel(IPrefsFront front)
        {
            Value = front.Theme.Connect().AddTo(_disposables);

            Toggle = new ReactiveCommand<Unit>().AddTo(_disposables);
            Toggle.SubscribeAwait(
                async (x, ct) => await front.SetThemeAsync(front.Theme.CurrentValue == Theme.Light ? Theme.Dark : Theme.Light, ct),
                AwaitOperation.Drop);
        }

        public ReadOnlyReactiveProperty<Theme> Value { get; }

        public ReactiveCommand<Unit> Toggle { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
