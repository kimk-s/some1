using System;
using Some1.Play.Info;
using R3;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectSpecialty : IObjectSpecialty
    {
        private readonly ReactiveProperty<SpecialtyId?> _id = new();
        private readonly ReactiveProperty<int> _number = new();
        private readonly ReactiveProperty<bool> _isPinned = new();
        private readonly ReactiveProperty<int> _token = new();

        internal ObjectSpecialty(int index)
        {
            Index = index;
        }

        public int Index { get; }

        public ReadOnlyReactiveProperty<SpecialtyId?> Id => _id;

        public ReadOnlyReactiveProperty<int> Number => _number;

        private void SetNumber(int value)
        {
            _number.Value = Math.Clamp(value, 0, int.MaxValue);
        }

        public ReadOnlyReactiveProperty<bool> IsPinned => _isPinned;

        public ReadOnlyReactiveProperty<int> Token => _token;

        internal void Add(SpecialtyId id, int number, int token)
        {
            if (number <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(number));
            }

            SetNumber(Id.CurrentValue != id ? number : Number.CurrentValue + number);
            _id.Value = id;
            _token.Value = token;
        }

        internal void Pin(bool value, int token)
        {
            _isPinned.Value = value;
            _token.Value = token;
        }

        internal void Reset()
        {
            _id.Value = null;
            SetNumber(0);
            _isPinned.Value = false;
            _token.Value = 0;
        }
    }
}
