using System.Threading;

namespace Some1
{
    public static class CancellationTokenSourceExtensions
    {
        public static void CancelSafely(this CancellationTokenSource cts)
        {
            if (cts.IsCancellationRequested)
            {
                return;
            }

            try
            {
                cts.Cancel();
            }
            catch
            {
            }
        }
    }
}
