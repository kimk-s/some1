using System.Net.Sockets;

namespace Some1
{
    public static class SocketExtensions
    {
        public static void ShutdownSafely(this Socket socket, SocketShutdown how)
        {
            try
            {
                socket.Shutdown(how);
            }
            catch
            {
            }
        }
    }
}
