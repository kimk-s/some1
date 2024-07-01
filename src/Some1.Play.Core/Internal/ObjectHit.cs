using System;
using System.Buffers;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;
using R3;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectHit : IObjectHit, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly ReactiveProperty<HitPacket?> _packet = new();
        private readonly ReactiveProperty<FloatWave> _cycles = new();
        private readonly ObjectEnergyGroup _energies;

        internal ObjectHit(ObjectEnergyGroup energies, ITime time)
        {
            _energies = energies;
            _sync = new SyncArraySource(
                _packet.ToNullableUnmanagedParticleSource(),
                _cycles.ToWaveSource(time));
        }

        public HitId? Id => _packet.Value?.Id;

        public int Value => _packet.Value?.Value ?? 0;

        public int RootId => _packet.Value?.RootId ?? 0;

        public float Angle => _packet.Value?.Angle ?? 0;

        public BirthId? BirthId { get; private set; }

        public int Token { get; private set; }

        public FloatWave Cycles => _cycles.Value;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal void Set(HitId id, int value, int rootId, float angle, BirthId? birthId, int token)
        {
            _packet.Value = new(id, value, rootId, angle);
            BirthId = birthId;
            Token = token;
            _cycles.Value = default;

            _energies.Add(EnergyId.Health, id.IsRecovery() ? value : - value);
        }

        internal void Update(float deltaSeconds)
        {
            if (deltaSeconds <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds));
            }

            if (Id is null)
            {
                return;
            }

            _cycles.Value = _cycles.Value.Flow(deltaSeconds / PlayConst.HitSeconds);

            if (_cycles.Value.B > 1f)
            {
                Reset();
            }
        }

        internal void Reset()
        {
            _packet.Value = null;
            BirthId = null;
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
