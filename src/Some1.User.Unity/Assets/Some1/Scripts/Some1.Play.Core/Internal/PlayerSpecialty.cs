using System;
using System.Buffers;
using MemoryPack;
using R3;
using Some1.Play.Data;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class PlayerSpecialty : IPlayerSpecialty, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly ReactiveProperty<int> _number = new();
        private readonly ReactiveProperty<DateTime> _numberUtc = new();

        internal PlayerSpecialty(SpecialtyInfo info)
        {
            Id = info.Id;

            _sync = new SyncArraySource(
                _number.ToUnmanagedParticleSource(),
                _numberUtc.ToUnmanagedParticleSource());
        }

        public SpecialtyId Id { get; }

        public int Number
        {
            get => _number.Value;

            private set
            {
                value = Math.Clamp(value, 0, int.MaxValue);
                if (value == Number)
                {
                    return;
                }

                _number.Value = value;

                Star = checked((byte)new Leveling(value, PlayConst.SpecialtyStarLeveling_MaxLevel, PlayConst.SpecialtyStarLeveling_StepFactor, LevelingMethod.Plain).Level);
            }
        }

        public byte Star { get; private set; }

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal SpecialtyData Save()
        {
            return new(
                (int)Id,
                Number,
                _numberUtc.Value);
        }

        public void AddNumber(int value, DateTime utc)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            if (value == 0)
            {
                return;
            }

            Number += value;
            _numberUtc.Value = utc;
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

        internal void Reset()
        {
            Number = 0;
        }
    }
}
