using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using R3;
using Some1.UI;
using Some1.Wait.Front;

namespace Some1.User.ViewModel
{
    public sealed class PlayPremiumLogGroupViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly CancellationTokenSource _cts = new();

        public PlayPremiumLogGroupViewModel(IWaitFront front)
        {
            _cts.AddTo(_disposables);

            Value = front.PremiumLogs
                .Select(x => x
                    ?.OrderByDescending(x => x.CreatedDate)
                    .Select(y => new PlayPremiumLogViewModel(
                        y,
                        y.PurchaseProductId is null ? null : front.Products.GetValueOrDefault(y.PurchaseProductId)))
                    .ToArray())
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            var fetchState = new ReactiveProperty<TaskState>().AddTo(_disposables);
            FetchState = fetchState;

            Fetch = new ReactiveCommand<Unit>();
            Fetch.SubscribeAwait(
                async (_, ct) => await front.FetchPremiumLogsAsync(ct),
                AwaitOperation.Drop,
                fetchState);
        }

        public ReadOnlyReactiveProperty<PlayPremiumLogViewModel[]?> Value { get; }

        public ReadOnlyReactiveProperty<TaskState> FetchState { get; }

        public ReactiveCommand<Unit> Fetch { get; }

        public void Dispose()
        {
            if (_disposables.IsDisposed)
            {
                return;
            }

            _cts.Cancel();
            _disposables.Dispose();
        }
    }
}
