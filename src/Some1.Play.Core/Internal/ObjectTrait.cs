using System;
using Some1.Play.Info;
using R3;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectTrait : IObjectTrait
    {
        private readonly Trait[] _buffValues = new Trait[PlayConst.BuffCount];
        private readonly ReactiveProperty<Trait> _next = new();
        private readonly ReactiveProperty<Trait> _afterNext = new();
        private Trait _castValue;
        private bool _nextUsed;

        public Trait Value { get; private set; }

        public ReadOnlyReactiveProperty<Trait> Next => _next;

        public ReadOnlyReactiveProperty<Trait> AfterNext => _afterNext;

        internal void Start()
        {
            Reset();
            Value = Trait.Common;
            _next.Value = NewTrait();
            _afterNext.Value = NewTrait();
        }

        internal void SetCastValue(bool next)
        {
            var value = next ? UseNext() : Trait.None;
            if (_castValue == value)
            {
                return;
            }
            _castValue = value;
            UpdateValue();
        }

        internal void SetBuffValue(int index, bool next)
        {
            var value = next ? UseNext() : Trait.None;
            if (_buffValues[index] == value)
            {
                return;
            }
            _buffValues[index] = value;
            UpdateValue();
        }

        internal void Update()
        {
            if (_nextUsed)
            {
                _nextUsed = false;
                _next.Value = _afterNext.Value;
                _afterNext.Value = NewTrait();
            }
        }

        internal void Reset()
        {
            foreach (ref var item in _buffValues.AsSpan())
            {
                item = Trait.None;
            }
            _castValue = Trait.None;
            _nextUsed = false;
            Value = Trait.None;
            _next.Value = Trait.None;
            _afterNext.Value = Trait.None;
        }

        private static Trait NewTrait() => RandomForUnity.Next(2) switch
        {
            0 => Trait.One,
            1 => Trait.Two,
            _ => throw new InvalidOperationException()
        };

        private Trait UseNext()
        {
            _nextUsed = true;
            return Next.CurrentValue;
        }

        private void UpdateValue()
        {
            Value = Trait.Common;
            foreach (var item in _buffValues)
            {
                Value |= item;
            }
            Value |= _castValue;
        }
    }
}
