namespace Some1.Play.Core.Internal
{
    internal sealed class TriggerState
    {
        internal TriggerSourceState Source { get; } = new();
        internal TriggerEventState Event { get; } = new();
    }
}
