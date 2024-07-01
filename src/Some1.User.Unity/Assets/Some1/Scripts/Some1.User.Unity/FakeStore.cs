using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Some1.Wait.Front;
using R3;
using System.Linq;

namespace Some1.User.Unity
{
    public sealed class FakeStore : IStore, IDisposable
    {
        private readonly Subject<IStoreProduct> _singleBuyProcessed = new();

        public FakeStore()
        {
            Products = new IStoreProduct[]
            {
                new FakeStoreProduct(
                    "premium30d",
                    "9,900원",
                    "프리미엄 30일",
                    "",
                    "KRW",
                    9900),
                new FakeStoreProduct(
                    "premium90d",
                    "23,700원",
                    "프리미엄 90일",
                    "",
                    "KRW",
                    23700),
                new FakeStoreProduct(
                    "premium180d",
                    "44,500원",
                    "프리미엄 180일",
                    "",
                    "KRW",
                    44500),
            }.ToDictionary(x => x.Id);
        }

        public IReadOnlyDictionary<string, IStoreProduct> Products { get; }

        public Observable<IStoreProduct> SingleBuyProcessed => _singleBuyProcessed;

        public Task<IStoreProduct> BuyAsync(string productId, CancellationToken cancellationToken)
        {
            var product = Products[productId];
            ((FakeStoreProduct)product).TransactionId = Guid.NewGuid().ToString();
            return Task.FromResult(product);
        }

        public Task ConfirmBuyAsync(string productId, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _singleBuyProcessed.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
