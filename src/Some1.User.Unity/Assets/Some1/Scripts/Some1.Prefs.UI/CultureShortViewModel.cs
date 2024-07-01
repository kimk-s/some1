using System;
using R3;
using Some1.Prefs.Front;

namespace Some1.Prefs.UI
{
    public sealed class CultureShortViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public CultureShortViewModel(
            Culture id,
            IPrefsFront front)
        {
            Id = id;

            Select = new ReactiveCommand<Unit>().AddTo(_disposables);
            Select.SubscribeAwait(
                async (_, ct) => await front.SetCultureAsync(id, ct),
                AwaitOperation.Drop);
        }

        public Culture Id { get; }

        public ReactiveCommand<Unit> Select { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
