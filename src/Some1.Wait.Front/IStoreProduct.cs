namespace Some1.Wait.Front
{
    public interface IStoreProduct
    {
        string Id { get; }
        string LocalizedPriceString { get; }
        string LocalizedTitle { get; }
        string LocalizedDescription { get; }
        string IsoCurrencyCode { get; }
        decimal LocalizedPrice { get; }
        string? TransactionId { get; }
    }

    // https://forum.unity.com/threads/clarity-on-retrieving-product-name-from-iap-api.1254945/
    public static class StoreProductExt
    {
        // For some reason the Unity IAP system calls the product's "name" property "title".
        // Here's an extension to simplify it.
        public static string GetLocalizedName(this IStoreProduct product)
        {
            string name = product.LocalizedTitle;

#if UNITY_ANDROID
            // On Android, the Play store appends the app name in parentheses to the defined name value.
            // For example, if the product name is "1,000 Gems" for an app named "Gem Collector", it will return "1,000 Gems (Gem Collector)".
            // Since the appended app name isn't wanted, trim it off before returning the value.
            int lastParen = name.LastIndexOf("(");
            if (lastParen > -1)
            {
                name = name.Substring(0, lastParen - 1).Trim();
            }
#endif

            return name;
        }
    }
}
