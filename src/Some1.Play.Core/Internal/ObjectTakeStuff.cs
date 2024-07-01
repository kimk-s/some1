using System.Buffers;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;
using R3;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectTakeStuff : IObjectTakeStuff, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly ReactiveProperty<Stuff?> _stuff = new();
        private readonly ReactiveProperty<FloatWave> _cycles = new();

        public ObjectTakeStuff(ITime time)
        {
            _sync = new SyncArraySource(
                _stuff.ToNullableUnmanagedParticleSource(),
                _cycles.ToWaveSource(time));
        }

        public Stuff? Stuff => _stuff.Value;

        public int Token { get; private set; }

        public FloatWave Cycles => _cycles.Value;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal void Set(Stuff stuff, int token)
        {
            _stuff.Value = stuff;
            Token = token;
            _cycles.Value = default;
        }

        internal void Update(float deltaSeconds)
        {
            if (Stuff is null)
            {
                return;
            }

            _cycles.Value = _cycles.Value.Flow(deltaSeconds / PlayConst.TakeStuffSeconds);
            if (Cycles.B > 1)
            {
                Reset();
            }
        }

        internal void Reset()
        {
            _stuff.Value = null;
            Token = 0;
            _cycles.Value = default;
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
