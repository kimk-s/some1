using System;
using System.Buffers;
using MemoryPack;
using Some1.Sync;
using R3;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class PlayerPremium : IPlayerPremium, ISyncSource
    {
        private readonly UnmanagedParticleSource<DateTime> _sync;
        private readonly ReactiveProperty<DateTime> _endTime = new();
        private readonly PlayerCharacterGroup _characters;
        private readonly ITime _time;
        private bool _isPremium;

        public PlayerPremium(PlayerCharacterGroup characters, ITime time)
        {
            _characters = characters;
            _time = time;
            _sync = _endTime.ToUnmanagedParticleSource();
        }

        public bool IsPremium
        {
            get => _isPremium;

            private set
            {
                if (_isPremium == value)
                {
                    return;
                }

                _isPremium = value;
                _characters.IsPremium = value;
            }
        }

        public DateTime EndTime
        {
            get => _endTime.Value;

            private set
            {
                if (_endTime.Value == value)
                {
                    return;
                }

                _endTime.Value = value;
                UpdateIsPremium();
            }
        }

        private void UpdateIsPremium()
        {
            IsPremium = EndTime >= _time.UtcNow;
        }

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal void Set(DateTime? endTime)
        {
            EndTime = endTime ?? DateTime.MinValue;
        }

        internal void Add(int days)
        {
            if (days < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(days));
            }

            var endTime = IsPremium ? EndTime : _time.UtcNow;
            EndTime = endTime.AddDays(days).AddMinutes(1);
        }

        internal void Update()
        {
            if (!IsPremium)
            {
                return;
            }

            UpdateIsPremium();
        }

        internal void Reset()
        {
            EndTime = DateTime.MinValue;
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
