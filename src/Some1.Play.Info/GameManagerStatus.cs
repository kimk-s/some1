namespace Some1.Play.Info
{
    public enum GameManagerStatus
    {
        Ready,
        Return,
        ReReady,
        ReturnFaulted,
    }

    public static class GameManagerStatusExtensions
    {
        public static bool IsRuning(this GameManagerStatus x)
        {
            return x == GameManagerStatus.Ready || x == GameManagerStatus.Return || x == GameManagerStatus.ReReady;
        }

        public static bool IsCancellable(this GameManagerStatus x)
        {
            return x == GameManagerStatus.Ready || x == GameManagerStatus.Return;
        }

        public static bool IsRuning(this GameManagerStatus? x)
        {
            return x is not null && x.Value.IsRuning();
        }

        public static bool IsCancellable(this GameManagerStatus? x)
        {
            return x is not null && x.Value.IsCancellable();
        }
    }
}
