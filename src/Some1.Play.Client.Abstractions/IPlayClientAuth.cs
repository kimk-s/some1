using System.Threading;
using System.Threading.Tasks;

namespace Some1.Play.Client
{
    public interface IPlayClientAuth
    {
        string GetUserId();
        Task<string> GetIdTokenAsync(bool forceRefresh, CancellationToken cancellationToken);
    }
}
