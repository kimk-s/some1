using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using Some1.UI;
using Some1.Wait.Front;

namespace Some1.User.ViewModel
{
    public sealed class WaitPlayServerGroupViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly CompositeDisposable _itemsDisposable = new();

        public WaitPlayServerGroupViewModel(
            IWaitFront front,
            ReactiveProperty<TaskState> executingState,
            ReactiveProperty<bool> sharedCanExecute)
        {
            _itemsDisposable.AddTo(_disposables);

            var selectCompleted = new ReactiveProperty<bool>().AddTo(_disposables);
            SelectCompleted = selectCompleted;

            var select = new ReactiveCommand<WaitPlayServerFrontId>(sharedCanExecute, true).AddTo(_disposables);
            select.SubscribeAwait(
                sharedCanExecute,
                async (id, ct) =>
                {
                    await front.SelectPlayServerAsync(id, ct);

                    selectCompleted.Value = true;
                },
                AwaitOperation.Drop,
                executingState);

            Items = front.PlayServers
                .Select(x =>
                {
                    _itemsDisposable.Clear();

                    return (IReadOnlyList<WaitPlayServerViewModel>?)x?.Values
                        .Select(y => new WaitPlayServerViewModel(y, select).AddTo(_itemsDisposable))
                        .ToArray();
                })
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            Fetch = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);
            Fetch.SubscribeAwait(
                sharedCanExecute,
                async (_, ct) => await front.FetchPlaysAsync(ct),
                AwaitOperation.Drop);

            IsBackable = front.SelectedPlayServer.Select(x => x is not null).ToReadOnlyReactiveProperty().AddTo(_disposables);

            Back = new ReactiveCommand<Unit>(IsBackable, true).AddTo(_disposables);
        }

        public ReadOnlyReactiveProperty<bool> SelectCompleted { get; }

        public ReadOnlyReactiveProperty<IReadOnlyList<WaitPlayServerViewModel>?> Items { get; }

        public ReactiveCommand<Unit> Fetch { get; }

        public ReadOnlyReactiveProperty<bool> IsBackable { get; }

        public ReactiveCommand<Unit> Back { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
