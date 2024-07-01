using System;
using System.Net.Sockets;

namespace Some1
{
    public static class ExceptionExtensions
    {
        public static string ToShortString(this Exception ex)
        {
            return ex switch
            {
                null => "{null}",
                SocketException socketEx => $"{ex.GetType().Name}: [{socketEx.SocketErrorCode} {socketEx.ErrorCode}] {ex.Message}",
                _ => $"{ex.GetType().Name}: {ex.Message}"
            };
        }
    }
}
