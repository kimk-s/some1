using Google.Apis.AndroidPublisher.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Microsoft.Extensions.Logging;

namespace Some1.Store.Admin
{
    public sealed class StoreAdmin : IStoreAdmin, IDisposable
    {
        public const string PackageName = "com..some1a";
        private readonly ILogger<StoreAdmin> _logger;
        private AndroidPublisherService? _service;

        private AndroidPublisherService Service => _service ?? throw new InvalidOperationException();

        public StoreAdmin(ILogger<StoreAdmin> logger)
        {
            _logger = logger;
        }

        public async Task InitializeAsync(string credentialPath, CancellationToken cancellationToken)
        {
            var credential = await GoogleCredential.FromFileAsync(credentialPath, CancellationToken.None);

            _service = new AndroidPublisherService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
            });
        }

        public async Task<StorePurchase> GetPurchaseAsync(string productId, string purchaseToken, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GET PUR {productId} {purchaseToken}");

            try
            {
                var purchase = await Service.Purchases.Products
                .Get(PackageName, productId, purchaseToken)
                .ExecuteAsync(cancellationToken);

                _logger.LogInformation($"GET PUR END {productId} {purchaseToken} {purchase.OrderId}");

                return new()
                {
                    Id = purchase.OrderId,
                    Date = DateTimeOffset.FromUnixTimeMilliseconds(purchase.PurchaseTimeMillis ?? 0).UtcDateTime
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET PUR ERR");
                throw;
            }
        }

        public void Dispose()
        {
            _service?.Dispose();
        }
    }
}
