namespace Some1.Play.Core
{
    public interface IObjectHitReached : ITriggerEventSource<EmptyTriggerEventArgs>
    {
        bool Value { get; }
    }
}
