using System;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectHitReached : IObjectHitReached
    {
        private readonly ObjectHitGroup _hits;
        private HitReachedInfo? _info;

        internal ObjectHitReached(ObjectHitGroup hits)
        {
            _hits = hits;
        }

        public bool Value { get; private set; }

        public event EventHandler<(EmptyTriggerEventArgs e, ParallelToken parallelToken)>? EventFired;

        public event EventHandler? ScopedReset;

        internal void Set(HitReachedInfo? info)
        {
            if (_info == info)
            {
                return;
            }

            _info = info;
            Clear();
        }

        internal void Update(ParallelToken parallelToken)
        {
            if (_info is null)
            {
                return;
            }

            if (Value)
            {
                return;
            }

            if (IsReached())
            {
                EventFired?.Invoke(this, (default, parallelToken));
                ScopedReset?.Invoke(this, EventArgs.Empty);
                Value = true;
            }
        }

        internal void Clear()
        {
            Value = false;
            _hits.ClearAttributeValues();
        }

        internal void Reset()
        {
            Clear();
            _info = null;
            ScopedReset?.Invoke(this, EventArgs.Empty);
        }

        private bool IsReached()
        {
            if (_info is null)
            {
                return false;
            }

            return _hits.GetAttributeValue(_info.Attribute) >= _info.Value;
        }
    }
}
