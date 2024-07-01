namespace Some1.Play.Core
{
    public enum PlayerDataStatus
    {
        None,
        BeginLoad,
        EndLoad,
        BeginSave,
        EndSave,
    }

    public static class PlayerDataStatusExtensions
    {
        public static bool IsLoaded(this PlayerDataStatus status) => status >= PlayerDataStatus.EndLoad;

        public static bool IsSaved(this PlayerDataStatus status) => status >= PlayerDataStatus.EndSave;

        public static bool IsRunning(this PlayerDataStatus status) => status == PlayerDataStatus.BeginLoad || status == PlayerDataStatus.BeginSave;
    }
}
