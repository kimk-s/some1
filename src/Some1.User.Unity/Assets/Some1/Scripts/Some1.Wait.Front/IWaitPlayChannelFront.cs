namespace Some1.Wait.Front
{
    public interface IWaitPlayChannelFront
    {
        int Number { get; }
        string PlayId { get; }
        bool OpeningSoon { get; }
        bool Maintenance { get; }
        float Busy { get; }
        bool IsFull { get; }
    }
}
