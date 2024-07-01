using System;
using System.Threading;
using System.Threading.Tasks;

namespace Some1.Store.Admin
{
    public sealed class FakeStoreAdmin : IStoreAdmin
    {
        public Task InitializeAsync(string credentialPath, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task<StorePurchase> GetPurchaseAsync(string productId, string purchaseToken, CancellationToken cancellationToken)
        {
            return Task.FromResult(new StorePurchase()
            {
                Id = $"FID.{productId}.{purchaseToken}",
                Date = DateTime.UtcNow,
            });
        }
    }
}
