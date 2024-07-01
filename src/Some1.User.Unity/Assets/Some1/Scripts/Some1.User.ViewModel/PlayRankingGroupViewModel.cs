using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using Some1.Play.Front;

namespace Some1.User.ViewModel
{
    public sealed class PlayRankingGroupViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayRankingGroupViewModel(IRankingGroupFront front)
        {
            All = front.All.Select(x => new PlayRankingViewModel(x).AddTo(_disposables)).ToArray();
            TimeLeftUntilUpdate = front.TimeLeftUntilUpdate;
            Empty = All.Select(x => x.Active).CombineLatestValuesAreAllFalse().ToReadOnlyReactiveProperty();
        }

        public IReadOnlyList<PlayRankingViewModel> All { get; }

        public ReadOnlyReactiveProperty<int> TimeLeftUntilUpdate { get; }

        public ReadOnlyReactiveProperty<bool> Empty { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
