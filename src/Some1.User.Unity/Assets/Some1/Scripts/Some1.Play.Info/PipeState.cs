using System;
using System.Collections.Generic;

namespace Some1.Play.Info
{
    public enum PipeStatus
    {
        None,
        Starting,
        Processing,
        Finishing,
        Finished,
        Terminated,
        Faulted,
    }

    public readonly struct PipeState : IEquatable<PipeState>
    {
        public PipeState(PipeStatus status) : this(status, null)
        {
        }

        public PipeState(PipeStatus status, Exception? exception)
        {
            Status = status;
            Exception = exception;
        }

        public PipeStatus Status { get; }

        public Exception? Exception { get; }

        public bool IsRunning => Status == PipeStatus.Starting || Status == PipeStatus.Processing || Status == PipeStatus.Finishing;

        public bool IsCompleted => Status == PipeStatus.Finished || Status == PipeStatus.Terminated || Status == PipeStatus.Faulted;

        public bool IsCompletedSuccessfully => Status == PipeStatus.Finished || Status == PipeStatus.Terminated;

        public override bool Equals(object? obj) => obj is PipeState other && Equals(other);

        public bool Equals(PipeState other) => Status == other.Status && EqualityComparer<Exception?>.Default.Equals(Exception, other.Exception);

        public override int GetHashCode() => HashCode.Combine(Status, Exception);

        public static bool operator ==(PipeState left, PipeState right) => left.Equals(right);

        public static bool operator !=(PipeState left, PipeState right) => !(left == right);
    }
}
