namespace Some1.Play.Core
{
    public interface ILeader
    {
        bool IsEnabled { get; }
        LeaderStatus Status { get; }
        ILeaderToken Token { get; }
    }

    public enum LeaderStatus
    {
        None,
        Lead1Completed,
        Lead2Completed,
        Lead3Completed,
    }
}
