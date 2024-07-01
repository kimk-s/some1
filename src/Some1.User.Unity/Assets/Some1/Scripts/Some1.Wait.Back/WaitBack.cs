using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Some1.Store.Admin;
using Some1.Wait.Data;

namespace Some1.Wait.Back
{
    public sealed class WaitBack : IWaitBack
    {
        private readonly IWaitRepository _repository;
        private readonly IStoreAdmin _storeAdmin;

        public WaitBack(IWaitRepository repository, IStoreAdmin storeAdmin)
        {
            _repository = repository;
            _storeAdmin = storeAdmin;
        }

        public async Task<WaitUserBack> GetUserAsync(string id, CancellationToken cancellationToken)
        {
            var userData = await _repository.GetUserAsync(id, cancellationToken).ConfigureAwait(false);
            var user = new WaitUserBack(userData.Id, userData.PlayId, userData.IsPlaying, userData.Manager);
            return user;
        }

        public async Task<WaitWaitBack> GetWaitAsync(CancellationToken cancellationToken)
        {
            var waitData = await _repository.GetWaitAsync(cancellationToken).ConfigureAwait(false);
            var wait = new WaitWaitBack(waitData.Maintenance);
            return wait;
        }

        public async Task<WaitPlayBack[]> GetPlaysAsync(CancellationToken cancellationToken)
        {
            var playDatas = await _repository.GetPlaysAsync(cancellationToken).ConfigureAwait(false);
            var plays = playDatas.Select(x => new WaitPlayBack(
                x.Id,
                x.Region,
                x.City,
                x.Number,
                x.Address,
                x.OpeningSoon,
                x.Maintenance,
                x.Busy)).ToArray();
            return plays;
        }

        public async Task<WaitPremiumLogBack[]> GetPremiumLogsAsync(string userId, CancellationToken cancellationToken)
        {
            var orderDatas = await _repository.GetPremiumLogsAsync(userId, cancellationToken).ConfigureAwait(false);
            var orders = orderDatas
                .Select(x => new WaitPremiumLogBack(
                    x.Id,
                    x.UserId,
                    (WaitPremiumLogReason)x.Reason,
                    x.CreatedDate,
                    x.PremiumChangedDays,
                    x.PremiumExpirationDate,
                    x.PurchaseOrderId,
                    x.PurchaseProductId,
                    x.PurchaseDate,
                    x.Note))
                .ToArray();
            return orders;
        }

        public async Task BuyProductAsync(string productId, string transactionId, string userId, CancellationToken cancellationToken)
        {
            var purchase = await _storeAdmin.GetPurchaseAsync(productId, transactionId, cancellationToken).ConfigureAwait(false);
            int premium = StoreProducts.GetPremium(productId);

            await _repository.BuyProductAsync(
                userId,
                productId,
                purchase.Id,
                purchase.Date,
                premium,
                cancellationToken);
        }
    }
}
