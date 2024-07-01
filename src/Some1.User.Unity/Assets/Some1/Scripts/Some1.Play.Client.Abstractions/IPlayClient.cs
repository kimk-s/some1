using System.Threading;
using System.Threading.Tasks;
using Some1.Net;

namespace Some1.Play.Client
{
    public interface IPlayClient
    {
        Task<DuplexPipe> StartPipeAsync(CancellationToken cancellationToken);
    }
}
