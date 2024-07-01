using System;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;
using Some1.UI;

namespace Some1.User.ViewModel
{
    public sealed class PlayUnaryViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayUnaryViewModel(
            IPlayFront front,
            Predicate<UnaryType> resultFilter, 
            SharedCanExecute sharedCanExecute)
        {
            IsRunning = front.IsUnaryRunning.Connect().AddTo(_disposables);

            Result = front.Player.Unary.Result
                .Where(x => x?.Unary is null || resultFilter(x.Value.Unary.Value.Type))
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            ClearResult = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);
            ClearResult.SubscribeAwait(
                sharedCanExecute,
                async (_, ct) => await front.ClearUnaryAsync(ct),
                AwaitOperation.Drop);
        }

        public ReadOnlyReactiveProperty<bool> IsRunning { get; }

        public ReadOnlyReactiveProperty<UnaryResult?> Result { get; }

        public ReactiveCommand<Unit> ClearResult { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
