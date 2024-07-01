using Grpc.Net.Client;

namespace Some1.Wait.Back.MagicClient
{
    public interface IGrpcChannelGettable
    {
        GrpcChannel GetGrpcChannel();
    }
}