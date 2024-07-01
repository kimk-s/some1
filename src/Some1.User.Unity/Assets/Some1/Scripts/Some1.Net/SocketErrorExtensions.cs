using System.Net.Sockets;

namespace Some1.Net
{
    public static class SocketErrorExtensions
    {
        public static bool IsKnown(this SocketError x)
        {
            return x switch
            {
                SocketError.ConnectionAborted => true,
                SocketError.ConnectionReset => true,
                SocketError.TimedOut => true,
                _ => false
            };
        }
    }
}
