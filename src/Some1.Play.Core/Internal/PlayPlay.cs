using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Some1.Play.Data;

namespace Some1.Play.Core.Internal
{
    internal sealed class PlayPlay : IDisposable
    {
        private readonly CancellationTokenSource _cts = new();
        private readonly ILogger<PlayPlay> _logger;
        private readonly string _id;
        private readonly PlayBusy _busy;
        private readonly IPlayRepository _repository;
        private readonly PlayerGroup _players;
        private Task<PlayPlayData>? _get;
        private Task? _set;

        internal PlayPlay(
            ILogger<PlayPlay> logger,
            string id,
            PlayBusy busy,
            IPlayRepository repository,
            PlayerGroup players)
        {
            logger.LogInformation("Id: {id}", id);

            _logger = logger;
            _id = id;
            _busy = busy;
            _repository = repository;
            _players = players;
        }

        internal void Update1()
        {
            Get();
        }

        internal void Update2()
        {
            Set();
        }

        private void Get()
        {
            _get ??= _repository.GetPlayAsync(_id, _cts.Token);

            if (_get.IsCompleted)
            {
                if (_get.IsCompletedSuccessfully)
                {
                    _players.SetMaintenance(_get.Result.Maintenance);
                    _get = null;
                }
                else
                {
                    _logger.LogError(_get.Exception, "Failed to get play to repository.");
                    _get = null;
                }
            }
        }

        private void Set()
        {
            _set ??= _repository.SetPlayAsync(GetData(), _cts.Token);

            if (_set.IsCompleted)
            {
                if (_set.IsCompletedSuccessfully)
                {
                    _set = null;
                }
                else
                {
                    _logger.LogError(_set.Exception, "Failed to set play to repository.");
                    _set = null;
                }
            }
        }

        private PlayPlayData GetData()
        {
            return new()
            {
                Id = _id,
                Busy = _busy.Value,
            };
        }

        public void Dispose()
        {
            try
            {
                _cts.Cancel();
            }
            catch
            {
            }
            _cts.Dispose();
        }
    }
}
