using System.Threading;
using System.Threading.Tasks;

namespace Some1.Wait.Back
{
    public interface IWaitBack
    {
        Task<WaitUserBack> GetUserAsync(string id, CancellationToken cancellationToken);
        Task<WaitWaitBack> GetWaitAsync(CancellationToken cancellationToken);
        Task<WaitPlayBack[]> GetPlaysAsync(CancellationToken cancellationToken);
        Task<WaitPremiumLogBack[]> GetPremiumLogsAsync(string userId, CancellationToken cancellationToken);
        Task BuyProductAsync(string productId, string transactionId, string userId, CancellationToken cancellationToken);
    }
}
