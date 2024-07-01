using System.Threading;
using System.Threading.Tasks;

namespace Some1.Wait.Back.MagicClient
{
    public interface IMagicClientWaitBackAuth
    {
        Task<string> GetIdTokenAsync(bool forceRefresh, CancellationToken cancellationToken);
    }
}
