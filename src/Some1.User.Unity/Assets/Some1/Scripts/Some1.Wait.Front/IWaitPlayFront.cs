namespace Some1.Wait.Front
{
    public interface IWaitPlayFront
    {
        string Id { get; }
        string Region { get; }
        string City { get; }
        int Number { get; }
        string Address { get; }
        bool OpeningSoon { get; }
        bool Maintenance { get; }
        float Busy { get; }
        bool IsFull { get; }
    }
}
