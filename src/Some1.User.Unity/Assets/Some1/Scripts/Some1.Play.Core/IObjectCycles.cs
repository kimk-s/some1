using R3;

namespace Some1.Play.Core
{
    public interface IObjectCycles : ITriggerEventSource<CycleSpan>
    {
        ReadOnlyReactiveProperty<FloatWave> Value { get; }
        float Cycle { get; }
        float Time { get; }
        bool CanUpdate { get; }
    }
}
