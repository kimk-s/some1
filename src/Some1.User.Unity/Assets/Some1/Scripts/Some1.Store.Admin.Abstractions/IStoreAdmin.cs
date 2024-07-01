using System.Threading;
using System.Threading.Tasks;

namespace Some1.Store.Admin
{
    public interface IStoreAdmin
    {
        Task InitializeAsync(string credentialPath, CancellationToken cancellationToken);
        Task<StorePurchase> GetPurchaseAsync(string productId, string purchaseToken, CancellationToken cancellationToken);
    }
}
