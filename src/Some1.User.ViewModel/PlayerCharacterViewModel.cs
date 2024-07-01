using System;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;
using Some1.UI;

namespace Some1.User.ViewModel
{
    public sealed class PlayerCharacterViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayerCharacterViewModel(
            IPlayerCharacterFront front,
            IPlayFront playFront,
            MessageListViewModel messageList,
            SharedCanExecute sharedCanExecute)
        {
            Id = front.Id;
            IsUnlocked = front.IsUnlocked.Connect().AddTo(_disposables);
            IsPicked = front.IsPicked.Connect().AddTo(_disposables);
            IsSelected = front.IsSelected.Connect().AddTo(_disposables);
            Star = front.Star.Connect().AddTo(_disposables);
            ExpTimeAgo = front.ExpTimeAgo.Connect().AddTo(_disposables);

            Select = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);
            Select.SubscribeAwait(
                sharedCanExecute,
                async (_, ct) =>
                {
                    var error = await playFront.SelectCharacterAsync(Id, ct);

                    if (error != PlayFrontError.Success)
                    {
                        messageList.Add.Execute(error);
                    }
                },
                AwaitOperation.Drop);
        }

        public CharacterId Id { get; }

        public ReadOnlyReactiveProperty<bool> IsUnlocked { get; }

        public ReadOnlyReactiveProperty<bool> IsPicked { get; }

        public ReadOnlyReactiveProperty<bool> IsSelected { get; }

        public ReadOnlyReactiveProperty<Leveling> Star { get; }

        public ReadOnlyReactiveProperty<TimeSpan?> ExpTimeAgo { get; }

        public ReactiveCommand<Unit> Select { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
