using Some1.Wait.Front;

namespace Some1.User.Unity
{
    public sealed class FakeStoreProduct : IStoreProduct
    {
        public FakeStoreProduct(string id, string localizedPriceString, string localizedTitle, string localizedDescription, string isoCurrencyCode, decimal localizedPrice)
        {
            Id = id;
            LocalizedPriceString = localizedPriceString;
            LocalizedTitle = localizedTitle;
            LocalizedDescription = localizedDescription;
            IsoCurrencyCode = isoCurrencyCode;
            LocalizedPrice = localizedPrice;
        }

        public string Id { get; }

        public string LocalizedPriceString { get; }

        public string LocalizedTitle { get; }

        public string LocalizedDescription { get; }

        public string IsoCurrencyCode { get; }

        public decimal LocalizedPrice { get; }

        public string TransactionId { get; set; }
    }
}
