using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;
using Some1.UI;
using Some1.Wait.Front;

namespace Some1.User.ViewModel
{
    public sealed class PlayLocationDetailViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly Func<PlayFrontError> _getExitError;

        public PlayLocationDetailViewModel(
            IPlayFront front,
            ReadOnlyReactiveProperty<IWaitPlayFront?> server,
            SharedCanExecute sharedCanExecute)
        {
            SpaceSize = front.Space.Area.Size;

            Regions = front.Regions.All.ToDictionary(
                x => x.Key,
                x => new PlayRegionViewModel(x.Value).AddTo(_disposables));

            RegionSection = front.Player.Object.Region.Section;

            Position = front.Player.Object.Transform.Position.Connect().AddTo(_disposables);

            Server = server
                .Select(x => new PlayServerViewModel(x))
                .ToReadOnlyReactiveProperty(null!)
                .AddTo(_disposables);

            OpenExitPopup = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);

            MessageList = new MessageListViewModel().AddTo(_disposables);

            Exit = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);
            Exit.Subscribe(_ =>
            {
                var error = front.Exit();

                if (error != PlayFrontError.Success)
                {
                    MessageList.Add.Execute(error);
                }
            });

            _getExitError = front.GetExitError;
        }

        public SizeF SpaceSize { get; }

        public IReadOnlyDictionary<RegionId, PlayRegionViewModel> Regions { get; }

        public ReadOnlyReactiveProperty<IRegionSectionFront?> RegionSection { get; }

        public ReadOnlyReactiveProperty<Vector2> Position { get; }

        public ReadOnlyReactiveProperty<PlayServerViewModel> Server { get; }

        public ReactiveCommand<Unit> OpenExitPopup { get; }

        public MessageListViewModel MessageList { get; }

        public ReactiveCommand<Unit> Exit { get; }

        public PlayFrontError GetExitError() => _getExitError();

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }

    public sealed class PlayServerViewModel
    {
        public PlayServerViewModel(IWaitPlayFront? front)
        {
            Id = front?.Id ?? "";
            Region = front?.Region ?? "";
            City = front?.City ?? "";
            Number = front?.Number ?? 0;
        }

        public string Id { get; }

        public string Region { get; }

        public string City { get; }

        public int Number { get; }
    }
}
