using System;
using System.Buffers;
using System.Collections.Generic;
using MemoryPack;
using R3;

namespace Some1.Sync.Sources
{
    public abstract class SyncParticleSource<T> : ISyncSource
    {
        private readonly ReadOnlyReactiveProperty<bool> _isDefault;
        private readonly ReactiveProperty<bool> _dirty = new();
        private readonly IDisposable _subscription;

        public SyncParticleSource(ReadOnlyReactiveProperty<T?> value)
        {
            Value = value;

            _isDefault = value.Select(x => EqualityComparer<T?>.Default.Equals(x, default)).ToReadOnlyReactiveProperty();

            _subscription = value.Subscribe(x => _dirty.Value = true);
            ClearDirty();
        }

        public ReadOnlyReactiveProperty<bool> IsDefault => _isDefault;

        public ReadOnlyReactiveProperty<bool> Dirty => _dirty;

        protected ReadOnlyReactiveProperty<T?> Value { get; }

        public void ClearDirty()
        {
            _dirty.Value = false;
        }

        public void Dispose()
        {
            _isDefault.Dispose();
            _dirty.Dispose();
            _subscription.Dispose();
        }

        public void Write<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, SyncMode mode) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
        {
            if (mode == SyncMode.Dirty && !_dirty.Value)
            {
                throw new InvalidOperationException();
            }

            if (IsDefault.CurrentValue)
            {
                writer.WriteUnmanaged((byte)0);
            }
            else
            {
                writer.WriteUnmanaged((byte)1);
                WriteValue(ref writer);
            }
        }

        protected abstract void WriteValue<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>;
    }
}
