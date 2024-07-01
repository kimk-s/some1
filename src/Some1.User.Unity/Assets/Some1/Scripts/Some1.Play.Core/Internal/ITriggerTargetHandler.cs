using System;
using System.Collections.Generic;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal interface ITriggerTargetHandler
    {
        void Handle(ITriggerTargetInfo info, ObjectTargetInfo destinationInfo, ICollection<Object> destinations, ParallelToken parallelToken);
    }

    internal sealed class TriggerTargetHandler : ITriggerTargetHandler
    {
        private readonly TriggerEventSourceState _sourceState;
        private readonly ObjectHierarchy _hierarchy;
        private readonly Space _space;

        internal TriggerTargetHandler(TriggerEventSourceState sourceState, ObjectHierarchy hierarchy, Space space)
        {
            _sourceState = sourceState;
            _hierarchy = hierarchy;
            _space = space;
        }

        public void Handle(ITriggerTargetInfo info, ObjectTargetInfo objectTargetInfo, ICollection<Object> destinations, ParallelToken parallelToken)
        {
            var objectTarget = new ObjectTarget(objectTargetInfo, _sourceState.Source.Team);

            switch (info)
            {
                case HierarchyTriggerTargetInfo i:
                    {
                        _hierarchy.GetObjects(i.Value, objectTarget, destinations);
                    }
                    break;
                case SpaceTriggerTargetInfo i:
                    {
                        foreach (var item in _space.GetObjects(Area.From(i.Value, _sourceState.Source.Area), objectTarget, parallelToken))
                        {
                            destinations.Add(item);
                        }
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
