namespace Some1.Play.Core
{
    public interface IObjectIdle
    {
        bool Idle { get; }
        IObjectCycles Cycles { get; }
    }
}
