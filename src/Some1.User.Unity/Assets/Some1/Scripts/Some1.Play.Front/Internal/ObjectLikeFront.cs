using System;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class ObjectLikeFront : IObjectLikeFront, IDisposable
    {
        private const float Seconds = 3;
        private const float Speed = 1 / Seconds;

        private readonly ReactiveProperty<bool> _value = new();
        private readonly ReactiveProperty<float> _cycles = new();

        public ReadOnlyReactiveProperty<bool> Value => _value;

        public ReadOnlyReactiveProperty<float> Cycles => _cycles;

        public void Dispose()
        {
            _value.Dispose();
            _cycles.Dispose();
        }

        internal void Set()
        {
            _value.Value = true;
            _cycles.Value = 0;
        }

        internal void Reset()
        {
            _value.Value = false;
            _cycles.Value = 0;
        }

        internal void Update(float deltaSeconds)
        {
            if (!_value.Value)
            {
                return;
            }

            _cycles.Value += deltaSeconds * Speed;
        }
    }
}
