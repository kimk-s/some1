using System;
using System.Collections.Generic;
using ObservableCollections;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class MessageListViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly ObservableDictionary<Guid, MessageListItemViewModel> _items = new();
        private bool _disposing;

        public MessageListViewModel()
        {
            Add = new ReactiveCommand<object>().AddTo(_disposables);
            Add.Subscribe(x =>
            {
                var item = new MessageListItemViewModel(this, x);
                _items.Add(item.Id, item);
            });
        }

        public IObservableCollection<KeyValuePair<Guid, MessageListItemViewModel>> Items => _items;

        public ReactiveCommand<object> Add { get; }

        public void Dispose()
        {
            _disposing = true;

            foreach (var item in _items)
            {
                item.Value.Dispose();
            }

            _disposables.Dispose();
        }

        internal void Remove(Guid id)
        {
            if (_disposing)
            {
                return;
            }

            _items.Remove(id);
        }
    }
}
