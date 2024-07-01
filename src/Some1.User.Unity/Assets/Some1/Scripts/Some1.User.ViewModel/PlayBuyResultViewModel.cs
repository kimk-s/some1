using System;
using Some1.Wait.Front;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class PlayBuyResultViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayBuyResultViewModel(IWaitFront front)
        {
            Result = front.BuyProductResult.Connect().AddTo(_disposables);

            ClearResult = new ReactiveCommand<Unit>().AddTo(_disposables);
            ClearResult.Subscribe(_ => front.ClearBuyProductResult());
        }

        public ReadOnlyReactiveProperty<WaitBuyProductResult> Result { get; }

        public ReactiveCommand<Unit> ClearResult { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
