using System;
using System.Collections.Generic;

namespace Some1.Play.Info
{
    public sealed class TriggerInfo
    {
        public TriggerInfo(
            TriggerId id,
            ITriggerEventInfo @event,
            TriggerConditionInfo condition,
            ITriggerTargetInfo target,
            ObjectTargetInfo objectTarget,
            TriggerDestinationUniqueInfo destinationUnique,
            ITriggerCommandInfo command) : this(
                id,
                @event,
                condition,
                target,
                objectTarget,
                destinationUnique,
                command,
                Array.Empty<TriggerPostInfo>())
        {
        }

        public TriggerInfo(
            TriggerId id,
            ITriggerEventInfo @event,
            TriggerConditionInfo condition,
            ITriggerTargetInfo target,
            ObjectTargetInfo objectTarget,
            TriggerDestinationUniqueInfo destinationUnique,
            ITriggerCommandInfo command,
            TriggerPostInfo post) : this(
                id,
                @event,
                condition,
                target,
                objectTarget,
                destinationUnique,
                command,
                post is null ? Array.Empty<TriggerPostInfo>() : new[] { post })
        {
        }

        public TriggerInfo(
            TriggerId id,
            ITriggerEventInfo @event,
            TriggerConditionInfo condition,
            ITriggerTargetInfo target,
            ObjectTargetInfo objectTarget,
            TriggerDestinationUniqueInfo destinationUnique,
            ITriggerCommandInfo command,
            IReadOnlyList<TriggerPostInfo> posts)
        {
            Id = id;
            Event = @event ?? throw new ArgumentNullException(nameof(@event));
            Condition = condition;
            Target = target ?? throw new ArgumentNullException(nameof(target));
            ObjectTarget = objectTarget;
            DestinationUnique = destinationUnique ?? throw new ArgumentNullException(nameof(destinationUnique));
            Command = command ?? throw new ArgumentNullException(nameof(command));
            Posts = posts;
        }

        public TriggerId Id { get; }

        public ITriggerEventInfo Event { get; }

        public TriggerConditionInfo Condition { get; }

        public ITriggerTargetInfo Target { get; }

        public ObjectTargetInfo ObjectTarget { get; }

        public TriggerDestinationUniqueInfo DestinationUnique { get; }

        public ITriggerCommandInfo Command { get; }

        public IReadOnlyList<TriggerPostInfo>? Posts { get; }
    }

    public sealed class TriggerPostInfo
    {
        public TriggerPostInfo(
            TriggerPostConditionInfo condition,
            ITriggerTargetInfo target,
            ObjectTargetInfo destination,
            TriggerDestinationUniqueInfo destinationUnique,
            ITriggerCommandInfo command)
        {
            Condition = condition;
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Destination = destination;
            DestinationUnique = destinationUnique ?? throw new ArgumentNullException(nameof(destinationUnique));
            Command = command ?? throw new ArgumentNullException(nameof(command));
        }

        public TriggerPostConditionInfo Condition { get; }

        public ITriggerTargetInfo Target { get; }

        public ObjectTargetInfo Destination { get; }

        public TriggerDestinationUniqueInfo DestinationUnique { get; }

        public ITriggerCommandInfo Command { get; }
    }

    public enum TriggerPostConditionInfo
    {
        PerHandled,
        Once,
    }
}
