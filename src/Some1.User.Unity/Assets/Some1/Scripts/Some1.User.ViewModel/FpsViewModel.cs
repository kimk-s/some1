using System;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class FpsViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public FpsViewModel()
        {
            SetClientFps = new ReactiveCommand<int>().AddTo(_disposables);
            SetSyncFps = new ReactiveCommand<int>().AddTo(_disposables);
            ClientFps = SetClientFps.ToReadOnlyReactiveProperty().AddTo(_disposables);
            SyncFps = SetSyncFps.ToReadOnlyReactiveProperty().AddTo(_disposables);
        }

        public ReadOnlyReactiveProperty<int> ClientFps { get; }

        public ReadOnlyReactiveProperty<int> SyncFps { get; }

        public ReactiveCommand<int> SetClientFps { get; }

        public ReactiveCommand<int> SetSyncFps { get; }

        public void Dispose() => _disposables.Dispose();
    }
}
