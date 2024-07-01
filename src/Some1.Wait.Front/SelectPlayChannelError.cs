using System;

namespace Some1.Wait.Front
{
    public enum WaitFrontError
    {
        None,
        ServerOpeningSoon,
        ServerMaintenance,
        ServerFull,
        ChannelOpeningSoon,
        ChannelMaintenance,
        ChannelFull,
    }

    public class WaitFrontException : Exception
    {
        public WaitFrontException(WaitFrontError error)
        {
            Error = error;
        }

        public WaitFrontException(WaitFrontError error, string? message) : base(message)
        {
            Error = error;
        }

        public WaitFrontException(WaitFrontError error, string? message, Exception? innerException) : base(message, innerException)
        {
            Error = error;
        }

        public WaitFrontError Error { get; }
    }
}
