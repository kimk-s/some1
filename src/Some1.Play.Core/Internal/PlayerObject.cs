using System.Buffers;
using System.Linq;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;
using R3;

namespace Some1.Play.Core.Internal
{
    internal sealed class PlayerObject : ISyncSource
    {
        private readonly SyncArraySource _sync;
        
        public PlayerObject(IObject @object, ITime time)
        {
            _sync = new SyncArraySource(
                new ReactiveProperty<int>(@object.Id).ToUnmanagedParticleSource(),
                @object.Character.Id.ToNullableUnmanagedParticleSource(),
                @object.Battle.Battle.ToNullableUnmanagedParticleSource(),
                new SyncArraySource(
                    @object.Alive.Alive.ToUnmanagedParticleSource(),
                    @object.Alive.Cycles.Value.ToWaveSource(time)),
                new SyncArraySource(@object.Cast.Items.Values.OrderBy(x => x.Id).Select(x => x.LoadWave.ToWaveSource(time))),
                new SyncArraySource(
                    @object.Specialties.Select(x => x.Id
                        .CombineLatest(
                            x.Number, x.IsPinned, x.Token,
                            (id, number, isPinned, token) => id is null ? (SpecialtyPacket?)null : new SpecialtyPacket(id.Value, number, isPinned, token))
                        .ToReadOnlyReactiveProperty()
                        .ToNullableUnmanagedParticleSource())),
                @object.TakeStuffs.ComboScore.ToUnmanagedParticleSource(),
                new SyncArraySource(@object.Energies.Values.OrderBy(x => x.Id).Select(x => new SyncArraySource(x.MaxValue.ToUnmanagedParticleSource(), x.Value.ToUnmanagedParticleSource()))),
                new SyncArraySource(
                    @object.Transform.Position.ToWaveSource(time),
                    @object.Transform.Rotation.ToUnmanagedParticleSource()),
                new SyncArraySource(
                    @object.Trait.Next.ToUnmanagedParticleSource(),
                    @object.Trait.AfterNext.ToUnmanagedParticleSource()),
                @object.Properties.Team.ToUnmanagedParticleSource());
        }

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
    }
}
