using System;
using System.Threading;
using System.Threading.Tasks;
using Some1.Data.InMemory;

namespace Some1.Wait.Data.InMemory
{
    public class InMemoryWaitRepository : IWaitRepository
    {
        private readonly InMemoryRepository _repository;

        public InMemoryWaitRepository(InMemoryRepository repository)
        {
            _repository = repository;
        }

        public Task<WaitPlayData[]> GetPlaysAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_repository.GetWaitPlays());
        }

        public Task<WaitWaitData> GetWaitAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_repository.GetWaitWait());
        }

        public Task<WaitUserData> GetUserAsync(string id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_repository.GetWaitUser(id));
        }

        public Task<WaitPremiumLogData[]> GetPremiumLogsAsync(string userId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_repository.GetPremiumLogs(userId));
        }

        public Task BuyProductAsync(string userId, string productId, string orderId, DateTime purchaseDate, int premium, CancellationToken cancellationToken)
        {
            _repository.BuyProduct(userId, productId, orderId, purchaseDate, premium);
            return Task.CompletedTask;
        }
    }
}
