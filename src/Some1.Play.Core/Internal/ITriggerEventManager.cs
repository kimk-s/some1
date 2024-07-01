using System;
using System.Numerics;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal interface ITriggerEventManager : IDisposable
    {
        TriggerEventSourceState State { get; }
        event EventHandler<ParallelToken>? EventFired;
        event EventHandler? ScopedReset;

        int Contains(ITriggerEventInfo info);
    }

    internal sealed class TriggerEventSourceState
    {
        internal TriggerEventState Event { get; } = new();
        internal TriggerSourceState Source { get; } = new();

        internal void Reset()
        {
            Event.Reset();
            Source.Reset();
        }
    }

    internal sealed class TriggerEventState
    {
        internal float CycleLength { get; set; }

        internal void Reset()
        {
            CycleLength = 0;
        }
    }

    internal sealed class TriggerSourceState
    {
        internal int RootId { get; set; }
        internal SkinId? SkinId { get; set; }
        internal int OffenseStat { get; set; }
        internal Trait Trait { get; set; }
        internal Trait NextTrait { get; set; }
        internal byte Team { get; set; }
        internal Area Area { get; set; }
        internal Aim Aim { get; set; }
        internal Vector2? Anchor { get; set; }
        internal BirthId? BirthId { get; set; }

        internal void Reset()
        {
            RootId = 0;
            SkinId = null;
            OffenseStat = 0;
            Trait = default;
            NextTrait = default;
            Team = default;
            Area = default;
            Aim = default;
            Anchor = null;
            BirthId = null;
        }
    }

    internal abstract class TriggerEventManager<TEventArgs, TInfo> : ITriggerEventManager
        where TEventArgs : struct
        where TInfo : ITriggerEventInfo
    {
        private readonly ITriggerEventSource<TEventArgs> _source;
        private readonly Action<TriggerSourceState> _setSourceState;
        private bool _disposed;

        public TriggerEventManager(ITriggerEventSource<TEventArgs> source, Action<TriggerSourceState> setSourceState)
        {
            _source = source;
            _source.EventFired += Source_EventFired;
            _source.ScopedReset += Source_ScopedReset;
            _setSourceState = setSourceState;
        }

        public TriggerEventSourceState State { get; } = new();

        public TEventArgs? Current { get; private set; }

        public event EventHandler<ParallelToken>? EventFired;

        public event EventHandler? ScopedReset;

        public int Contains(ITriggerEventInfo info)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (Current is null)
            {
                throw new InvalidOperationException();
            }

            return Contains((TInfo)info, Current.Value);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _source.EventFired -= Source_EventFired;
                _source.ScopedReset -= Source_ScopedReset;

                _disposed = true;
            }
        }

        protected virtual bool ContinueEvent() => true;

        protected abstract int Contains(TInfo info, TEventArgs e);

        protected abstract void SetEventState(TriggerEventState eventState, TEventArgs e);

        private void Source_EventFired(object? _, (TEventArgs e, ParallelToken parallelToken) e)
        {
            Current = e.e;

            if (ContinueEvent())
            {
                SetEventState(State.Event, e.e);
                _setSourceState(State.Source);
                EventFired?.Invoke(this, e.parallelToken);
            }
        }

        private void Source_ScopedReset(object? _, EventArgs __)
        {
            ScopedReset?.Invoke(this, EventArgs.Empty);
        }
    }

    internal sealed class CycleTriggerEventManager : TriggerEventManager<CycleSpan, CycleTriggerEventInfo>
    {
        public CycleTriggerEventManager(ITriggerEventSource<CycleSpan> source, Action<TriggerSourceState> setSourceState)
            : base(source, setSourceState)
        {
        }

        protected override int Contains(CycleTriggerEventInfo info, CycleSpan e)
        {
            return e.Contains(info.Repeat);
        }

        protected override void SetEventState(TriggerEventState eventState, CycleSpan e)
        {
            eventState.CycleLength = e.Length;
        }
    }

    internal sealed class EmptyTriggerEventManager : TriggerEventManager<EmptyTriggerEventArgs, EmptyTriggerEventInfo>
    {
        public EmptyTriggerEventManager(ITriggerEventSource<EmptyTriggerEventArgs> source, Action<TriggerSourceState> setSourceState)
            : base(source, setSourceState)
        {
        }

        protected override int Contains(EmptyTriggerEventInfo info, EmptyTriggerEventArgs e)
        {
            return 1;
        }

        protected override void SetEventState(TriggerEventState eventState, EmptyTriggerEventArgs e)
        {
        }
    }

    internal sealed class PostTriggerEventManager : TriggerEventManager<EmptyTriggerEventArgs, EmptyTriggerEventInfo>
    {
        public PostTriggerEventManager(ITriggerEventSource<EmptyTriggerEventArgs> source, Action<TriggerSourceState> setSourceState)
            : base(source, setSourceState)
        {
        }

        protected override int Contains(EmptyTriggerEventInfo info, EmptyTriggerEventArgs e)
        {
            return 1;
        }

        protected override void SetEventState(TriggerEventState eventState, EmptyTriggerEventArgs e)
        {
        }
    }
}
