using Some1.Wait.Front;
using UnityEngine.Purchasing;

namespace Some1.User.Unity
{
    public sealed class UnityStoreProduct : IStoreProduct
    {
        private readonly Product _unityProduct;

        public UnityStoreProduct(Product unityProduct)
        {
            _unityProduct = unityProduct;
        }

        public string Id => _unityProduct.definition.id;

        public string LocalizedPriceString => _unityProduct.metadata.localizedPriceString;

        public string LocalizedTitle => string.IsNullOrEmpty(_unityProduct.metadata.localizedTitle) ? $"<{Id}>" : _unityProduct.metadata.localizedTitle;

        public string LocalizedDescription => _unityProduct.metadata.localizedDescription;

        public string IsoCurrencyCode => _unityProduct.metadata.isoCurrencyCode;

        public decimal LocalizedPrice => _unityProduct.metadata.localizedPrice;

        public string? TransactionId => _unityProduct.transactionID;

        public string? Receipt => _unityProduct.receipt;
    }
}
