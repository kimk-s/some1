using System;
using System.Buffers;
using MemoryPack;
using Some1.Sync;
using R3;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectLike : IObjectLike, ISyncSource
    {
        private readonly UnmanagedParticleSource<int> _sync;
        private readonly ReactiveProperty<int> _count = new();

        internal ObjectLike()
        {
            _sync = _count.ToUnmanagedParticleSource();
        }

        public int Count => _count.Value;

        public bool CanAdd => Enabled;

        internal bool Enabled { get; set; }

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public event EventHandler<Like>? Added;

        public void Reset()
        {
            if (!Enabled)
            {
                return;
            }

            _count.Value = 0;

            Enabled = false;
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

        internal bool Add(Like like)
        {
            if (!CanAdd)
            {
                return false;
            }

            _count.Value++;
            Added?.Invoke(this, like);

            return true;
        }
    }
}
