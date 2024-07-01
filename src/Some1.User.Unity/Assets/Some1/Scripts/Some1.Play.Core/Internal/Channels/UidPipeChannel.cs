using System.Threading.Channels;

namespace Some1.Play.Core.Internal.Channels
{
    internal sealed class UidPipeChannel : Channel<UidPipe>
    {
        internal UidPipeChannel()
        {
            var channel = Channel.CreateUnbounded<UidPipe>(
                new UnboundedChannelOptions()
                {
                    SingleReader = true,
                    SingleWriter = false,
                    AllowSynchronousContinuations = false,
                });
            Reader = channel;
            Writer = channel;
        }
    }
}
