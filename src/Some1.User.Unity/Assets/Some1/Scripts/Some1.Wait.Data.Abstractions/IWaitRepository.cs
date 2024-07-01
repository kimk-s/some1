using System;
using System.Threading;
using System.Threading.Tasks;

namespace Some1.Wait.Data
{
    public interface IWaitRepository
    {
        Task<WaitUserData> GetUserAsync(string id, CancellationToken cancellationToken);
        Task<WaitWaitData> GetWaitAsync(CancellationToken cancellationToken);
        Task<WaitPlayData[]> GetPlaysAsync(CancellationToken cancellationToken);
        Task<WaitPremiumLogData[]> GetPremiumLogsAsync(string userId, CancellationToken cancellationToken);
        Task BuyProductAsync(string userId, string productId, string orderId, DateTime purchaseDate, int premium, CancellationToken cancellationToken);
    }
}
