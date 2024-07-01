using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using R3;

namespace Some1.Wait.Front
{
    public interface IStore
    {
        IReadOnlyDictionary<string, IStoreProduct> Products { get; }

        Observable<IStoreProduct> SingleBuyProcessed { get; }

        Task StartAsync(CancellationToken cancellationToken);
        Task<IStoreProduct?> BuyAsync(string productId, CancellationToken cancellationToken);
        Task ConfirmBuyAsync(string productId, CancellationToken cancellationToken);
    }
}
