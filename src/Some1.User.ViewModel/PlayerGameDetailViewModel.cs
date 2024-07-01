using System;
using System.Linq;
using ObservableCollections;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;
using Some1.UI;

namespace Some1.User.ViewModel
{
    public sealed class PlayerGameDetailViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly ObservableList<PlayerGameSpecialtyViewModel> _specialtiesOrdered;
        private readonly Subject<Unit> _returnSuccess = new();

        public PlayerGameDetailViewModel(
            IPlayFront front,
            SharedCanExecute sharedCanExecute)
        {
            var gameManager = front.Player.GameManager;
            var game = front.Player.Game;
            var obj = front.Player.Object;

            Close = game.Game.Where(x => x is null).AsUnitObservable();

            Game = game.Game;

            Time = game.Game
                .CombineLatest(
                    game.Cycles,
                    (game, cycles) => game is null
                        ? TimeSpan.MinValue
                        : game.Value.Mode == GameMode.Challenge
                            ? TimeSpan.FromSeconds(MathF.Max(MathF.Ceiling(PlayConst.ChallengeSeconds - cycles), 0))
                            : TimeSpan.FromSeconds(cycles))
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            ManagerStatus = gameManager.Status;

            MessageList = new MessageListViewModel().AddTo(_disposables);

            _specialtiesOrdered = new ObservableList<PlayerGameSpecialtyViewModel>(obj.Specialties.All
                .Select(x => new PlayerGameSpecialtyViewModel(x, front, MessageList, sharedCanExecute)
                .AddTo(_disposables)));

            var ordered = _specialtiesOrdered
                .OrderByDescending(x => x.Specialty.CurrentValue?.IsPinned ?? false)
                .ThenByDescending(x => x.Specialty.CurrentValue?.Token ?? 0);

            foreach (var item in _specialtiesOrdered)
            {
                item.Specialty.Skip(1)
                    .Subscribe(x => _specialtiesOrdered.Move(
                        _specialtiesOrdered.IndexOf(item),
                        ordered.Select((x, n) => (x, n)).First(x => x.x == item).n));
            }

            SpecialtyActiveCount = obj.Specialties.ActiveCount.Connect().AddTo(_disposables);

            _returnSuccess.AddTo(_disposables);

            Return = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);
            Return.SubscribeAwait(
                sharedCanExecute,
                async (_, ct) =>
                {
                    var error = await front.ReturnGameAsync(ct);

                    if (error != PlayFrontError.Success)
                    {
                        MessageList.Add.Execute(error);
                    }
                    else
                    {
                        _returnSuccess.OnNext(Unit.Default);
                    }
                },
                AwaitOperation.Drop);
        }

        public Observable<Unit> Close { get; }

        public ReadOnlyReactiveProperty<Game?> Game { get; }

        public ReadOnlyReactiveProperty<TimeSpan> Time { get; }

        public ReadOnlyReactiveProperty<GameManagerStatus?> ManagerStatus { get; }

        public IObservableCollection<PlayerGameSpecialtyViewModel> SpecialtiesOrdered => _specialtiesOrdered;

        public ReadOnlyReactiveProperty<int> SpecialtyActiveCount { get; }

        public MessageListViewModel MessageList { get; }

        public ReactiveCommand<Unit> Return { get; }

        public Observable<Unit> ReturnSuccess => _returnSuccess;

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
