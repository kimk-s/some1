using System;
using System.Threading;
using System.Threading.Tasks;
using MagicOnion.Client;
using MagicOnion.Serialization;
using MagicOnion.Serialization.MemoryPack;

namespace Some1.Wait.Back.MagicClient
{
    public sealed class MagicClientWaitBack : IWaitBack
    {
        private const float TimeoutSeconds = 7;
        private readonly IGrpcChannelGettable _grpcChannelGettable;
        private readonly IMagicClientWaitBackAuth _auth;

        static MagicClientWaitBack()
        {
            MagicOnionSerializerProvider.Default = MemoryPackMagicOnionSerializerProvider.Instance;
        }

        public MagicClientWaitBack(IGrpcChannelGettable grpcChannelGettable, IMagicClientWaitBackAuth auth)
        {
            _grpcChannelGettable = grpcChannelGettable;
            _auth = auth;
        }

        private IWaitBackMagicService GetService()
        {
            return MagicOnionClient.Create<IWaitBackMagicService>(_grpcChannelGettable.GetGrpcChannel(), new IClientFilter[]
            {
                new AppendHeaderFilter(_auth),
            });
        }

        public async Task<WaitUserBack> GetUserAsync(string id, CancellationToken cancellationToken)
        {
            return await GetService()
                .WithCancellationToken(cancellationToken)
                .WithDeadline(DateTime.UtcNow.AddSeconds(TimeoutSeconds))
                .GetUserAsync();
        }

        public async Task<WaitWaitBack> GetWaitAsync(CancellationToken cancellationToken)
        {
            return await GetService()
                .WithCancellationToken(cancellationToken)
                .WithDeadline(DateTime.UtcNow.AddSeconds(TimeoutSeconds))
                .GetWaitAsync();
        }

        public async Task<WaitPlayBack[]> GetPlaysAsync(CancellationToken cancellationToken)
        {
            return await GetService()
                .WithCancellationToken(cancellationToken)
                .WithDeadline(DateTime.UtcNow.AddSeconds(TimeoutSeconds))
                .GetPlaysAsync();
        }

        public async Task<WaitPremiumLogBack[]> GetPremiumLogsAsync(string userId, CancellationToken cancellationToken)
        {
            return await GetService()
                .WithCancellationToken(cancellationToken)
                .WithDeadline(DateTime.UtcNow.AddSeconds(TimeoutSeconds))
                .GetOrdersAsync();
        }

        public async Task BuyProductAsync(string productId, string transactionId, string userId, CancellationToken cancellationToken)
        {
            await GetService()
                .WithCancellationToken(cancellationToken)
                .WithDeadline(DateTime.UtcNow.AddSeconds(TimeoutSeconds))
                .BuyProductAsync((productId, transactionId));
        }
    }
}
