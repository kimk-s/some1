using System;
using Cysharp.Net.Http;
using Grpc.Net.Client;
using Some1.Wait.Back.MagicClient;

namespace Some1.User.Unity
{
    public sealed class YetGrpcChannelGettable : IGrpcChannelGettable, IDisposable
    {
        private readonly YetAnotherHttpHandler _handler;
        private readonly GrpcChannel _channel;

        public YetGrpcChannelGettable(IWaitAddressGettable addressGettable)
        {
            _handler = new YetAnotherHttpHandler()
            {
                Http2Only = true,
            };

            _channel = GrpcChannel.ForAddress(
                addressGettable.GetAddress(),
                new GrpcChannelOptions() { HttpHandler = _handler });
        }

        public GrpcChannel GetGrpcChannel()
        {
            return _channel;
        }

        public void Dispose()
        {
            _channel.Dispose();
            _handler.Dispose();
        }
    }
}
