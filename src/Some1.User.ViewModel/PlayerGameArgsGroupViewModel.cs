using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;
using Some1.UI;

namespace Some1.User.ViewModel
{
    public sealed class PlayerGameArgsGroupViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayerGameArgsGroupViewModel(
            IPlayFront front,
            SharedCanExecute sharedCanExecute)
        {
            All = EnumForUnity.GetValues<GameMode>().ToDictionary(
                x => x,
                x => new PlayerGameArgsViewModel(x, front, sharedCanExecute).AddTo(_disposables));

            Close = Observable.Merge(All.Values.Select(x => x.Close));
        }

        internal Observable<Unit> Close { get; }

        public IReadOnlyDictionary<GameMode, PlayerGameArgsViewModel> All { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
