using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;
using Some1.UI;

namespace Some1.User.ViewModel
{
    public sealed class PlayerCharacterGroupViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly Func<CharacterId, PlayerCharacterDetailViewModel> _createPlayerCharacterDetailViewModel;

        public PlayerCharacterGroupViewModel(
            IPlayFront front,
            MessageListViewModel messageList,
            SharedCanExecute sharedCanExecute)
        {
            Items = front.Player.Characters.All.ToDictionary(
                x => x.Key,
                x => new PlayerCharacterViewModel(
                    x.Value,
                    front,
                    messageList,
                    sharedCanExecute)
                .AddTo(_disposables));

            PickTimeRemained = front.Player.Characters.PickTimeRemained.Connect().AddTo(_disposables);

            Star = front.Player.Characters.Star.AddTo(_disposables);

            _createPlayerCharacterDetailViewModel = id => new(front, id, messageList, sharedCanExecute);
        }

        public IReadOnlyDictionary<CharacterId, PlayerCharacterViewModel> Items { get; }

        public ReadOnlyReactiveProperty<TimeSpan> PickTimeRemained { get; }

        public ReadOnlyReactiveProperty<Leveling> Star { get; }

        public PlayerCharacterDetailViewModel CreateDetail(CharacterId id)
        {
            return _createPlayerCharacterDetailViewModel(id);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
