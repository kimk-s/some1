using System;
using R3;

namespace Some1.Prefs.UI
{
    public sealed class CultureLongViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public CultureLongViewModel(Culture id, ReactiveCommand<Culture> select)
        {
            Id = id;

            Select = new ReactiveCommand<Unit>().AddTo(_disposables);
            Select.Subscribe(_ => select.Execute(Id));
        }

        public Culture Id { get; }

        public ReactiveCommand<Unit> Select { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
