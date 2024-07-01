using System;

namespace Some1.User.ViewModel
{
    public sealed class MessageListItemViewModel : IDisposable
    {
        private readonly MessageListViewModel _list;
        private bool _disposed;

        internal MessageListItemViewModel(MessageListViewModel list, object message)
        {
            Id = Guid.NewGuid();
            _list = list;
            Message = message;
        }

        public Guid Id { get; }

        public object Message { get; }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _list.Remove(Id);
            _disposed = true;
        }
    }
}
