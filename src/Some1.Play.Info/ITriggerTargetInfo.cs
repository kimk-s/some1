using System;

namespace Some1.Play.Info
{
    public interface ITriggerTargetInfo
    {
    }

    public sealed class HierarchyTriggerTargetInfo : ITriggerTargetInfo
    {
        public HierarchyTriggerTargetInfo(HierarchyTarget value)
        {
            Value = value;
        }

        public HierarchyTarget Value { get; }
    }

    public sealed class SpaceTriggerTargetInfo : ITriggerTargetInfo
    {
        public SpaceTriggerTargetInfo(AreaInfo value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public AreaInfo Value { get; }
    }
}
