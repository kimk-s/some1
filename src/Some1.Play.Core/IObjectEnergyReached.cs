namespace Some1.Play.Core
{
    public interface IObjectEnergyReached : ITriggerEventSource<EmptyTriggerEventArgs>
    {
        float Delay { get; }
    }
}
