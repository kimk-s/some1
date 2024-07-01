using System;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectEnergyReached : IObjectEnergyReached
    {
        private readonly ObjectEnergyGroup _energies;
        private EnergyReachedInfo? _info;

        internal ObjectEnergyReached(ObjectEnergyGroup energies)
        {
            _energies = energies;
        }

        public float Delay { get; private set; }

        public event EventHandler<(EmptyTriggerEventArgs e, ParallelToken parallelToken)>? EventFired;

        public event EventHandler? ScopedReset;

        internal void Set(EnergyReachedInfo? info)
        {
            _info = info;
        }

        internal void Update(float deltaSeconds, ParallelToken parallelToken)
        {
            if (_info is null)
            {
                return;
            }

            if (Delay > 0)
            {
                Delay -= deltaSeconds;
                if (Delay <= 0)
                {
                    Delay = 0;
                }

                return;
            }

            if (IsReached())
            {
                EventFired?.Invoke(this, (default, parallelToken));
                ScopedReset?.Invoke(this, EventArgs.Empty);
                Delay = _info!.Delay;
            }
        }

        internal void Reset()
        {
            _info = null;
            Delay = default;
            ScopedReset?.Invoke(this, EventArgs.Empty);
        }

        private bool IsReached()
        {
            if (_info is null)
            {
                return false;
            }

            float left = _energies.All[_info.Id].NormalizedValue;
            float right = _info.NormalizedValue;

            return _info.Operator switch
            {
                EnergyReachedOperator.Equals => left == right,
                EnergyReachedOperator.Less => left < right,
                EnergyReachedOperator.LessOrEquals => left <= right,
                EnergyReachedOperator.Greater => left > right,
                EnergyReachedOperator.GreaterOrEquals => left >= right,
                _ => throw new InvalidOperationException()
            };
        }
    }
}
