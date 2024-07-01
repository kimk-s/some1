using System;
using System.Collections.Generic;

namespace Some1.UI
{
    public enum TaskStateType
    {
        None,
        Running,
        RanToCompletion,
        Faulted,
    }

    public readonly struct TaskState : IEquatable<TaskState>
    {
        public TaskState(TaskStateType type) : this(type, null)
        {
        }

        public TaskState(TaskStateType type, Exception? exception)
        {
            Type = type;
            Exception = exception;
        }

        public static TaskState None => default;

        public TaskStateType Type { get; }

        public Exception? Exception { get; }

        public bool IsRunning => Type == TaskStateType.Running;

        public bool IsCompleted => IsCompletedSuccessfully || IsFaulted;

        public bool IsCompletedSuccessfully => Type == TaskStateType.RanToCompletion;

        public bool IsFaulted => Type == TaskStateType.Faulted;

        public override bool Equals(object? obj) => obj is TaskState other && Equals(other);

        public bool Equals(TaskState other) => Type == other.Type && EqualityComparer<Exception?>.Default.Equals(Exception, other.Exception);

        public override int GetHashCode() => HashCode.Combine(Type, Exception);

        public static bool operator ==(TaskState left, TaskState right) => left.Equals(right);

        public static bool operator !=(TaskState left, TaskState right) => !(left == right);
    }
}
