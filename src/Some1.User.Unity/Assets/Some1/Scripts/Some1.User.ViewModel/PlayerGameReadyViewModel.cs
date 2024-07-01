using System;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;
using Some1.UI;

namespace Some1.User.ViewModel
{
    public sealed class PlayerGameReadyViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayerGameReadyViewModel(IPlayFront front, SharedCanExecute sharedCanExecute)
        {
            ManagerStatus = front.Player.GameManager.Status.Connect().AddTo(_disposables);
            ManagerCycles = front.Player.GameManager.Cycles.Connect().AddTo(_disposables);
            SelectedArgs = front.Player.GameArgses.Selected.Connect().AddTo(_disposables);
            Game = front.Player.Game.Game.Connect().AddTo(_disposables);
            ExpValue = front.Player.Exp.Value.Connect().AddTo(_disposables);
            ExpChargeTimeRemained = front.Player.Exp.ChargeTimeRemained.Connect().AddTo(_disposables);

            Ready = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);
            Ready.SubscribeAwait(
                sharedCanExecute,
                async (_, ct) => await front.ReadyGameAsync(ct),
                AwaitOperation.Drop);

            Cancel = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);
            Cancel.SubscribeAwait(
                sharedCanExecute,
                async (_, ct) => await front.CancelGameAsync(ct),
                AwaitOperation.Drop);
        }

        public ReadOnlyReactiveProperty<GameManagerStatus?> ManagerStatus { get; }

        public ReadOnlyReactiveProperty<float> ManagerCycles { get; }

        public ReadOnlyReactiveProperty<IPlayerGameArgsFront?> SelectedArgs { get; }

        public ReadOnlyReactiveProperty<Game?> Game { get; }

        public ReadOnlyReactiveProperty<int> ExpValue { get; }

        public ReadOnlyReactiveProperty<TimeSpan> ExpChargeTimeRemained { get; }

        public ReactiveCommand<Unit> Ready { get; }

        public ReactiveCommand<Unit> Cancel { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
