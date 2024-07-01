using Microsoft.Extensions.Hosting;
using Some1.Play.Core;

namespace Some1.Play.Server.Tcp
{
    internal class PlayLoopService : BackgroundService
    {
        private readonly PlayLoop _loop;

        public PlayLoopService(PlayLoop loop)
        {
            _loop = loop;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _loop.RunAsync(stoppingToken);
        }
    }
}
