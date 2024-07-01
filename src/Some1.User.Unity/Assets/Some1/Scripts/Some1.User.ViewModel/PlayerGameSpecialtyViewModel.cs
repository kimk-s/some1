using System;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;
using Some1.UI;

namespace Some1.User.ViewModel
{
    public sealed class PlayerGameSpecialtyViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayerGameSpecialtyViewModel(
            IPlayerObjectSpecialtyFront front,
            IPlayFront playFront,
            MessageListViewModel messageList,
            SharedCanExecute sharedCanExecute)
        {
            Specialty = front.Specialty;

            Pin = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);
            Pin.SubscribeAwait(
                sharedCanExecute,
                async (_, ct) =>
                {
                    if (front.Specialty.CurrentValue is null)
                    {
                        return;
                    }

                    var error = await playFront.PinSpecialtyAsync(front.Specialty.CurrentValue.Value.Id, !front.Specialty.CurrentValue.Value.IsPinned, ct);

                    if (error != PlayFrontError.Success)
                    {
                        messageList.Add.Execute(error);
                    }
                },
                AwaitOperation.Drop);
        }

        public ReadOnlyReactiveProperty<SpecialtyPacket?> Specialty { get; }

        public ReactiveCommand<Unit> Pin { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
