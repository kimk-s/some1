using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using Some1.Play.Front;
using Some1.UI;
using Some1.Wait.Front;

namespace Some1.User.ViewModel
{
    public sealed class PlayerPremiumViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly Func<PlayPremiumLogGroupViewModel> _createPremiumLogGroup;

        public PlayerPremiumViewModel(
            IPlayFront playFront,
            IWaitFront waitFront,
            SharedCanExecute sharedCanExecute)
        {
            EndTime = playFront.Player.Premium.EndTime.Connect().AddTo(_disposables);
            TimeLeft = playFront.Player.Premium.TimeLeft.Connect().AddTo(_disposables);

            var buyState = new ReactiveProperty<TaskState>().AddTo(_disposables);
            BuyState = buyState;

            var buy = new ReactiveCommand<string>(sharedCanExecute, true).AddTo(_disposables);
            buy.SubscribeAwait(
                sharedCanExecute,
                async (id, ct) => await waitFront.BuyProductAsync(id, ct),
                AwaitOperation.Drop,
                buyState);

            Products = waitFront.Products.Values
                .Select(x => new PlayerProductViewModel(x, buy).AddTo(_disposables))
                .ToDictionary(x => x.Id, x => x);

            _createPremiumLogGroup = () => new PlayPremiumLogGroupViewModel(waitFront);
        }

        public ReadOnlyReactiveProperty<DateTime> EndTime { get; }

        public ReadOnlyReactiveProperty<TimeSpan> TimeLeft { get; }

        public IReadOnlyDictionary<string, PlayerProductViewModel> Products { get; }

        public PlayPremiumLogGroupViewModel CreatePremiumLogGroup() => _createPremiumLogGroup();

        public ReactiveProperty<TaskState> BuyState { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
