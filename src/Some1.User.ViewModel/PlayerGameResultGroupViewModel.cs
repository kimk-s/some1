using System;
using System.Linq;
using ObservableCollections;
using R3;
using Some1.Play.Front;

namespace Some1.User.ViewModel
{
    public sealed class PlayerGameResultGroupViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly ObservableList<PlayerGameResultViewModel> _orderByDescendingEndTime;
        private readonly Func<int, PlayerGameResultDetailViewModel> _createDetail;

        public PlayerGameResultGroupViewModel(IPlayerGameResultGroupFront front)
        {
            _orderByDescendingEndTime = new ObservableList<PlayerGameResultViewModel>(front.All
                .Select((x, n) => new PlayerGameResultViewModel(n, x).AddTo(_disposables)));

            foreach (var item in _orderByDescendingEndTime)
            {
                item.EndTime.Skip(1).Subscribe(x => _orderByDescendingEndTime.Move(
                    _orderByDescendingEndTime.IndexOf(item),
                    _orderByDescendingEndTime.OrderByDescending(x => x.EndTime.CurrentValue)
                        .Select((x, n) => (x, n))
                        .First(x => x.x == item).n));
            }

            _createDetail = (int id) => new(front.All[id]);
        }

        public IObservableCollection<PlayerGameResultViewModel> OrderByDescendingEndTime => _orderByDescendingEndTime;

        public int GetIdOfLastEndTime()
        {
            return _orderByDescendingEndTime.OrderByDescending(x => x.EndTime.CurrentValue).First().Id;
        }

        public PlayerGameResultDetailViewModel CreateDetail(int id) => _createDetail(id);

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
