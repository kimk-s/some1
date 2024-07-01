using System;
using System.Buffers;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;
using R3;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectEnergy : IObjectEnergy, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly ReactiveProperty<int> _maxValue = new();
        private readonly ReactiveProperty<int> _value = new();
        private readonly CharacterEnergyInfoGroup _infos;
        private CharacterId? _characterId;
        private int _characterValue;
        private int _powerValue;

        internal ObjectEnergy(EnergyId id, CharacterEnergyInfoGroup infos)
        {
            Id = id;
            _infos = infos ?? throw new ArgumentNullException(nameof(infos));
            _sync = new SyncArraySource(
                _maxValue.ToUnmanagedParticleSource(),
                _value.ToUnmanagedParticleSource());
        }

        public EnergyId Id { get; }

        public ReadOnlyReactiveProperty<int> MaxValue => _maxValue;

        private void SetMaxValue(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            if (MaxValue.CurrentValue == value)
            {
                return;
            }
            var ratio = _value.Value == 0 ? 0 : (float)Value.CurrentValue / MaxValue.CurrentValue;
            _maxValue.Value = value;
            SetValue((int)MathF.Ceiling(value * ratio));
        }

        public ReadOnlyReactiveProperty<int> Value => _value;

        internal void SetValue(int value)
        {
            value = Math.Clamp(value, 0, MaxValue.CurrentValue);
            if (Value.CurrentValue == value)
            {
                return;
            }
            _value.Value = value;
        }

        public float NormalizedValue => MaxValue.CurrentValue == 0 ? 0 : (float)Value.CurrentValue / MaxValue.CurrentValue;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal void Set(CharacterId characterId)
        {
            if (_characterId == characterId)
            {
                return;
            }
            _characterId = characterId;
            _characterValue = _infos.ById.GetValueOrDefault(new(characterId, Id))?.Value ?? 0;
            UpdateMaxValue();
        }

        internal void SetStatValue(int value)
        {
            if (_powerValue == value)
            {
                return;
            }
            _powerValue = value;
            UpdateMaxValue();
        }

        internal void Reset()
        {
            _characterId = null;
            _characterValue = 0;
            _powerValue = 0;
            SetMaxValue(0);
            SetValue(0);
        }

        private void UpdateMaxValue()
        {
            SetMaxValue(StatHelper.Calculate(_characterValue, _powerValue));
        }

        public void ClearDirty()
        {
            _sync.ClearDirty();
        }

        public void Write<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, SyncMode mode) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
        {
            _sync.Write(ref writer, mode);
        }

        public void Dispose()
        {
            _sync.Dispose();
        }
    }
}
