namespace Some1.Play.Core
{
    public interface IControl
    {
        int TakenFrameCount { get; }
        bool CanTake { get; }
        bool IsTaken { get; }
    }
}
