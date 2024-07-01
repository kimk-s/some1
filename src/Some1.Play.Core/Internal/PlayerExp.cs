using System;
using System.Buffers;
using MemoryPack;
using Some1.Play.Data;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;
using R3;

namespace Some1.Play.Core.Internal
{
    internal sealed class PlayerExp : IPlayerExp, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly ReactiveProperty<int> _value = new();
        private readonly ReactiveProperty<DateTime> _chargeTime = new();
        private readonly ITime _time;

        internal PlayerExp(ITime time)
        {
            _time = time;
            _sync = new SyncArraySource(
                _value.ToUnmanagedParticleSource(),
                _chargeTime.ToUnmanagedParticleSource());
        }

        public int Value { get => _value.Value; private set => _value.Value = value; }

        public DateTime ChargeTime { get => _chargeTime.Value; private set => _chargeTime.Value = value; }

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        internal void Load(ExpData? data)
        {
            if (data is null)
            {
                Value = PlayConst.MaxPlayerExp;
                return;
            }

            Value = data.Value.Value;
            ChargeTime = data.Value.ChargeTime;
        }

        internal ExpData Save()
        {
            return new(
                Value,
                ChargeTime);
        }

        internal int Consume(int value)
        {
            int v = Math.Min(value, Value);
            Value -= v;
            if (ChargeTime == DateTime.MinValue)
            {
                ChargeTime = _time.UtcNow.AddMinutes(PlayConst.PlayerExpChargeMinutes);
            }
            return v;
        }

        internal void Reset()
        {
            Value = 0;
            ChargeTime = DateTime.MinValue;
        }

        internal void Update()
        {
            while (ChargeTime != DateTime.MinValue && ChargeTime <= _time.UtcNow)
            {
                int v = Math.Min(PlayConst.PlayerExpChargeValue, PlayConst.MaxPlayerExp - Value);
                Value += v;
                if (Value < PlayConst.MaxPlayerExp)
                {
                    ChargeTime = ChargeTime.AddMinutes(PlayConst.PlayerExpChargeMinutes);
                }
                else
                {
                    ChargeTime = DateTime.MinValue;
                }
            }
        }

        public void ClearDirty()
        {
            _sync.ClearDirty();
        }

        public void Dispose()
        {
            _sync.Dispose();
        }

        public void Write<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, SyncMode mode) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
        {
            _sync.Write(ref writer, mode);
        }
    }
}
