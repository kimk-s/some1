using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Some1.Wait.Front;
using R3;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Security;

namespace Some1.User.Unity
{
    public sealed class UnityStore : Wait.Front.IStore, IDetailedStoreListener, IDisposable
    {
        private readonly Subject<IStoreProduct> _singleBuyProcessed = new();
        private IStoreController _controller;
        private IReadOnlyDictionary<string, IStoreProduct>? _products;
        private TaskCompletionSource<bool>? _startTcs;
        private TaskCompletionSource<IStoreProduct?>? _buyTcs;
        private bool _disposed;

        public IReadOnlyDictionary<string, IStoreProduct> Products => _products ?? throw new InvalidOperationException();

        public Observable<IStoreProduct> SingleBuyProcessed => _singleBuyProcessed;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ThrowsIfDisposed();

            if (_startTcs is not null)
            {
                return _startTcs.Task;
            }
            _startTcs = new();

            var module = StandardPurchasingModule.Instance();
            module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;

            var builder = ConfigurationBuilder.Instance(module);
            builder.AddProduct("premium30d", ProductType.Consumable);
            builder.AddProduct("premium90d", ProductType.Consumable);
            builder.AddProduct("premium180d", ProductType.Consumable);

            var googleConfig = builder.Configure<IGooglePlayConfiguration>();
            googleConfig.SetDeferredPurchaseListener(GoogleDeferredPurchaseListener);

            UnityPurchasing.Initialize(this, builder);

            return _startTcs.Task;
        }

        public async Task<IStoreProduct?> BuyAsync(string productId, CancellationToken cancellationToken)
        {
            ThrowsIfDisposed();

            if (_buyTcs is not null)
            {
                throw new InvalidOperationException();
            }
            _buyTcs = new();

            try
            {
                _controller.InitiatePurchase(productId);
                return await _buyTcs.Task;
            }
            finally
            {
                _buyTcs?.TrySetCanceled();
                _buyTcs = null;
            }
        }

        public Task ConfirmBuyAsync(string productId, CancellationToken cancellationToken)
        {
            ThrowsIfDisposed();

            var product = _controller.products.WithID(productId);
            _controller.ConfirmPendingPurchase(product);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _startTcs?.TrySetCanceled();
            _buyTcs?.TrySetCanceled();
            _singleBuyProcessed.Dispose();

            _disposed = true;
        }

        void IStoreListener.OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _products = controller.products.all.Select(x => (IStoreProduct)new UnityStoreProduct(x))
                .ToDictionary(x => x.Id);
            _controller = controller;
            _startTcs.SetResult(true);
        }

        void IStoreListener.OnInitializeFailed(InitializationFailureReason error)
        {
            ((IStoreListener)this).OnInitializeFailed(error, null);
        }

        void IStoreListener.OnInitializeFailed(InitializationFailureReason error, string message)
        {
            var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

            if (message != null)
            {
                errorMessage += $" More details: {message}";
            }

            _startTcs.SetException(new Exception(errorMessage));
        }

        void IStoreListener.OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            ((IDetailedStoreListener)this).OnPurchaseFailed(product, new(product.definition.id, failureReason, null)); ;
        }

        void IDetailedStoreListener.OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            var exception = new Exception(
                $"Purchase failed - Product: '{product.definition.id}'," +
                $" Purchase failure reason: {failureDescription.reason}," +
                $" Purchase failure details: {failureDescription.message}");

            if (_buyTcs is null || !_buyTcs.TrySetException(exception))
            {
                Debug.Log($"Failed to set exception on OnPurchaseFailed. {exception}");
            }
        }

        PurchaseProcessingResult IStoreListener.ProcessPurchase(PurchaseEventArgs e)
        {
            try
            {
                ValidateReceipt(e.purchasedProduct.receipt);

                var product = Products[e.purchasedProduct.definition.id];

                if (_buyTcs is null || !_buyTcs.TrySetResult(product))
                {
                    _singleBuyProcessed.OnNext(product);
                }
            }
            catch (Exception ex)
            {
                _buyTcs?.TrySetException(ex);
            }

            return PurchaseProcessingResult.Pending;
        }

        private void ThrowsIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        private void ValidateReceipt(string receipt)
        {
//            if (Application.isEditor)
//            {
//                return;
//            }

//#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
//            var validator = new CrossPlatformValidator(GooglePlayTangle.Data(), null, Application.identifier);
//            var result = validator.Validate(receipt);
//            foreach (IPurchaseReceipt productReceipt in result)
//            {
//                if (productReceipt is GooglePlayReceipt google)
//                {
//                    if (google.purchaseState != GooglePurchaseState.Purchased)
//                    {
//                        throw new Exception($"Failed to Google Receipt. PurchaseState is not Purchased ({google.purchaseState}).");
//                    }
//                }
//            }
//#endif
        }

        private void GoogleDeferredPurchaseListener(Product product)
        {
            if (_buyTcs is null)
            {
                throw new InvalidOperationException();
            }

            _buyTcs.TrySetResult(null);
        }
    }
}
