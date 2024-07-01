using System.Collections.Generic;

namespace Some1.Wait.Front
{
    public interface IWaitPlayServerFront
    {
        WaitPlayServerFrontId Id { get; }
        IReadOnlyDictionary<int, IWaitPlayChannelFront> Channels { get; }
        bool OpeningSoon { get; }
        bool Maintenance { get; }
        bool IsFull { get; }
        bool IsSelected { get; }
    }
}
