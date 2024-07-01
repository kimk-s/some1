using System.Threading;
using System.Threading.Tasks;
using Some1.Net;
using Some1.Play.Core;

namespace Some1.Play.Client.InMemory
{
    public sealed class InMemoryPlayClient : IPlayClient
    {
        private readonly IPlayCore _core;
        private readonly IPlayClientAuth _auth;

        public InMemoryPlayClient(IPlayCore core, IPlayClientAuth authTokenGettable)
        {
            _core = core;
            _auth = authTokenGettable;
        }

        public async Task<DuplexPipe> StartPipeAsync(CancellationToken cancellationToken)
        {
            var userId = _auth.GetUserId();
            var pipePair = DuplexPipePairPool.Rent();
            await _core.AddUidPipeAsync(new(userId, pipePair.A), cancellationToken).ConfigureAwait(false);
            return pipePair.B;
        }
    }
}
