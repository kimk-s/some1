using System;
using System.Threading;
using System.Threading.Tasks;

namespace Some1.Play.Info
{
    public class MemoryPlayInfoRepository : IPlayInfoRepository
    {
        private readonly PlayInfo _value;

        public MemoryPlayInfoRepository(PlayInfo value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public PlayInfo Value { get; private set; } = null!;

        public Task LoadAsync(CancellationToken cancellationToken)
        {
            Value = _value;
            return Task.CompletedTask;
        }
    }
}
