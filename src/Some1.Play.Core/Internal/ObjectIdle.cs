using System.Buffers;
using MemoryPack;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;
using R3;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectIdle : IObjectIdle, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly CharacterIdleInfoGroup _infos;
        private readonly ReactiveProperty<bool> _idle = new();
        private readonly ObjectCycles _cycles;
        private readonly ObjectCast _cast;
        private CharacterId? _characterId;

        internal ObjectIdle(CharacterIdleInfoGroup infos, ObjectCast cast, ITime time)
        {
            _infos = infos;
            _cycles = new(time, (_, __) => true);
            _cast = cast;
            _sync = new SyncArraySource(
                _idle.ToUnmanagedParticleSource(),
                _cycles);
        }

        public bool Idle
        {
            get => _idle.Value;

            private set
            {
                if (Idle == value)
                {
                    return;
                }

                _idle.Value = value;

                UpdateInfo();
                _cast.SetIdle(value);
            }
        }

        public IObjectCycles Cycles => _cycles;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal void Set(CharacterId characterId)
        {
            if (_characterId == characterId)
            {
                return;
            }

            _characterId = characterId;

            UpdateInfo();
        }

        internal void Update(float deltaTime, ParallelToken parallelToken)
        {
            Idle = _cast.Current is null;

            _cycles.Update(deltaTime, parallelToken);
        }

        internal void Stop()
        {
            _cycles.Stop();
        }

        internal void Reset()
        {
            Idle = false;
            _cycles.Reset();
        }

        private void UpdateInfo()
        {
            var info = _characterId is null
                ? null
                : _infos.ById.GetValueOrDefault(new(_characterId.Value, Idle));

            _cycles.Cycle = info?.Cycle ?? 0;
            _cycles.Stop();
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
