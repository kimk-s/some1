using System.Buffers;
using MemoryPack;
using Some1.Play.Data;
using Some1.Sync;
using R3;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class PlayerWelcome : IPlayerWelcome, ISyncSource
    {
        private readonly UnmanagedParticleSource<bool> _sync;
        private readonly ReactiveProperty<bool> _welcome = new();

        public PlayerWelcome()
        {
            _sync = _welcome.ToUnmanagedParticleSource();
        }

        public bool Welcome { get => _welcome.Value; internal set => _welcome.Value = value; }

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

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

        internal void Load(WelcomeData? data)
        {
            if (data is null)
            {
                return;
            }

            Welcome = data.Value.Value;
        }

        internal WelcomeData Save()
        {
            return new(Welcome);
        }

        internal void Reset()
        {
            Welcome = false;
        }
    }
}
