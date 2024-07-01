//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.Purchasing;
//using UnityEngine.Purchasing.Extension;

//namespace Some1.User.Unity
//{
//    public class IAPManager : IDetailedStoreListener
//    {
//        private IStoreController _controller;
//        private IExtensionProvider _extensions;

//        public IAPManager()
//        {
//            var module = StandardPurchasingModule.Instance();
//            module.useFakeStoreUIMode = FakeStoreUIMode.Default;

//            var builder = ConfigurationBuilder.Instance(module);
//            builder.AddProduct("premium30d", ProductType.Consumable);
//            builder.AddProduct("premium90d", ProductType.Consumable);
//            builder.AddProduct("premium180d", ProductType.Consumable);

//            UnityPurchasing.Initialize(this, builder);

//            Debug.Log($"{nameof(IAPManager)}] {module.appStore} {module.Version} {module.useFakeStoreAlways} {module.useFakeStoreUIMode} {builder.useCatalogProvider}");
//        }

//        public void InitiatePurchase(string productId)
//        {
//            Debug.Log($"{nameof(InitiatePurchase)}] {productId}");
//            _controller.InitiatePurchase(productId);
//        }

//        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
//        {
//            Debug.Log($"{nameof(OnInitialized)}");
//            _controller = controller;
//            _extensions = extensions;

//            foreach (var item in controller.products.all)
//            {
//                Debug.Log($"{nameof(OnInitialized)}] {GetString(item)}");
//            }
//        }

//        public void OnInitializeFailed(InitializationFailureReason error, string message)
//        {
//            Debug.Log($"{nameof(OnInitializeFailed)}] {error} {message}");
//        }

//        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
//        {
//            Debug.Log($"{nameof(ProcessPurchase)}] {GetString(e.purchasedProduct)}");
//            return PurchaseProcessingResult.Complete;
//        }

//        public void OnPurchaseFailed(Product i, PurchaseFailureDescription p)
//        {
//            Debug.Log($"{nameof(OnPurchaseFailed)}] {p.productId} {p.reason} {p.message} {GetString(i)}");
//        }

//        [Obsolete]
//        void IStoreListener.OnInitializeFailed(InitializationFailureReason error) { }

//        [Obsolete]
//        void IStoreListener.OnPurchaseFailed(Product i, PurchaseFailureReason p) { }

//        private static string GetString(Product x)
//            => x is null ? "" : $"<{GetString(x.definition)},{GetString(x.metadata)},{x.availableToPurchase},{x.transactionID},{x.appleOriginalTransactionID},{x.appleProductIsRestored},{x.receipt}>";

//        private static string GetString(ProductDefinition x)
//            => x is null ? "" : $"<{x.id},{x.storeSpecificId},{x.type},{x.enabled},{GetString(x.payout)},{GetString(x.payouts)}>";

//        private static string GetString(ProductMetadata x)
//            => x is null ? "" : $"<{x.localizedPriceString},{x.localizedTitle},{x.localizedDescription},{x.isoCurrencyCode},{x.localizedPrice}>";

//        private static string GetString(IEnumerable<PayoutDefinition> x)
//            => x is null ? "" : $"<{string.Join(',',x.Select(x => GetString(x)))}>";

//        private static string GetString(PayoutDefinition x)
//            => x is null ? "" : $"<{x.type},{x.subtype},{x.quantity},{x.data}>";
//    }
//}
