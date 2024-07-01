using System;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectStat : IObjectStat
    {
        private readonly CharacterStatInfoGroup _infos;
        private readonly ObjectEnergyGroup _energies;
        private readonly int[] _buffValues = new int[PlayConst.BuffCount];
        private CharacterId? _characterId;
        private int _characterValue;
        private int _castValue;
        private int _boosterValue;
        private int _value;

        internal ObjectStat(StatId id, CharacterStatInfoGroup infos, ObjectEnergyGroup energies)
        {
            Id = id;
            _infos = infos ?? throw new ArgumentNullException(nameof(infos));
            _energies = energies;
        }

        public event EventHandler<int>? ValueChanged;

        public StatId Id { get; }

        public int Value
        {
            get => _value;
            private set
            {
                value = Math.Clamp(value, PlayConst.MinStatValue, PlayConst.MaxStatValue);
                if (_value == value)
                {
                    return;
                }
                _value = value;

                switch (Id)
                {
                    case StatId.Health:
                        _energies.SetStatValue(EnergyId.Health, value);
                        break;
                    case StatId.Mana:
                        _energies.SetStatValue(EnergyId.Mana, value);
                        break;
                    default:
                        break;
                }

                ValueChanged?.Invoke(this, value);
            }
        }

        internal void Set(CharacterId characterId)
        {
            if (_characterId == characterId)
            {
                return;
            }
            _characterId = characterId;
            _characterValue = _infos.ById.GetValueOrDefault(new(characterId, Id))?.Value ?? 0;
            UpdateValue();
        }

        internal void SetCastValue(int value)
        {
            if (_castValue == value)
            {
                return;
            }
            _castValue = value;
            UpdateValue();
        }

        internal void SetBuffValue(int value, int index)
        {
            if (_buffValues[index] == value)
            {
                return;
            }
            _buffValues[index] = value;
            UpdateValue();
        }

        internal void SetBoosterValue(int value)
        {
            if (_boosterValue == value)
            {
                return;
            }
            _boosterValue = value;
            UpdateValue();
        }

        internal void Reset()
        {
            _characterId = null;
            _characterValue = 0;
            _castValue = 0;
            foreach (ref var item in _buffValues.AsSpan())
            {
                item = 0;
            }
            _boosterValue = 0;
            Value = 0;
        }

        private void UpdateValue()
        {
            var buffsValue = 0;
            foreach (var item in _buffValues)
            {
                buffsValue += item;
            }
            Value = _characterValue + _castValue + buffsValue + _boosterValue;
        }
    }
}
