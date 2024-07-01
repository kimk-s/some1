using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using Some1.UI;
using Some1.Wait.Front;

namespace Some1.User.ViewModel
{
    public sealed class WaitPlayServerDetailViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public WaitPlayServerDetailViewModel(
            IWaitFront front,
            WaitPlayServerFrontId id,
            ReactiveProperty<TaskState> executingState,
            ReactiveProperty<bool> sharedCanExecute)
        {
            var server = front.PlayServers.CurrentValue![id];

            Id = server.Id;
            OpeningSoon = server.OpeningSoon;
            Maintenance = server.Maintenance;
            IsFull = server.IsFull;

            var select = new ReactiveCommand<int>(sharedCanExecute, true).AddTo(_disposables);
            select.SubscribeAwait(
                sharedCanExecute,
                async (x, ct) => await front.SelectPlayChannelAsync(x, ct),
                AwaitOperation.Drop,
                executingState);

            Channels = server.Channels.Values
                .Select(x => new WaitPlayChannelViewModel(x, select).AddTo(_disposables))
                .ToArray();

            Back = new ReactiveCommand<Unit>().AddTo(_disposables);
        }

        public WaitPlayServerFrontId Id { get; }

        public bool OpeningSoon { get; }

        public bool Maintenance { get; }

        public bool IsFull { get; }

        public IReadOnlyList<WaitPlayChannelViewModel> Channels { get; }

        public ReactiveCommand<Unit> Back { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
