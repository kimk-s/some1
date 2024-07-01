using System;
using System.Collections.Generic;
using MemoryPack;
using R3;

namespace Some1.Sync.Destinations
{
    public abstract class SyncParticleDestination<T> : ISyncDestination
    {
        private readonly ReactiveProperty<T?> _value = new();

        public SyncParticleDestination()
        {
            IsDefault = _value
                .Select(x => EqualityComparer<T?>.Default.Equals(x, default))
                .ToReadOnlyReactiveProperty();
        }

        public ReadOnlyReactiveProperty<T?> Value => _value;

        protected ReactiveProperty<T?> ProtectedValue => _value;

        public ReadOnlyReactiveProperty<bool> IsDefault { get; }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            byte code = reader.ReadUnmanaged<byte>();
            if (code == 0)
            {
                Reset();
            }
            else if (code == 1)
            {
                ReadValue(ref reader);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void Reset()
        {
            _value.Value = default;
        }

        public void Dispose()
        {
            _value.Dispose();
        }

        protected abstract void ReadValue(ref MemoryPackReader reader);
    }
}
