using System;

namespace Some1.Play.Info
{
    public interface ITriggerEventInfo
    {
    }

    public sealed class CycleTriggerEventInfo : ITriggerEventInfo
    {
        public CycleTriggerEventInfo(CycleRepeat repeat)
        {
            Repeat = repeat ?? throw new ArgumentNullException(nameof(repeat));
        }

        public CycleRepeat Repeat { get; }
    }

    public sealed class EmptyTriggerEventInfo : ITriggerEventInfo
    {
    }
}
