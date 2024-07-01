using System;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class ObjectShiftFront : IObjectShiftFront, ISyncDestination, ISyncPolatable
    {
        private readonly SyncArrayDestination _sync;
        private readonly NullableUnmanagedParticleDestination<Shift> _shift = new();
        private readonly FloatWaveDestination _cycles;

        internal ObjectShiftFront(ISyncTime syncFrame, ReadOnlyReactiveProperty<CharacterShiftInfo?> info)
        {
            _cycles = new(syncFrame);

            var topHeight = Shift
                .Select(x => x is null ? 0 : MathF.Min(x.Value.Time * 2, x.Value.Id.GetMaxHeight()));
            Height = Cycles
                .CombineLatest(
                    topHeight,
                    (cycles, topHeight) => (cycles < 0.5f ? cycles : (1 - cycles)) * 2 * topHeight)
                .ToReadOnlyReactiveProperty();

            _sync = new(
                _shift,
                _cycles);

            Cycle = info
                .CombineLatest(
                    Shift,
                    (info, shift) => info is null || shift is null ? 0 : shift.Value.Id switch
                    {
                        ShiftId.Jump => info.JumpCycle ?? 0,
                        ShiftId.Knock => info.KnockCycle ?? 0,
                        ShiftId.Dash => info.DashCycle ?? 0,
                        ShiftId.Grab => info.GrabCycle ?? 0,
                        _ => throw new InvalidOperationException(),
                    })
                .ToReadOnlyReactiveProperty();

            var time = Shift.CombineLatest(Cycles, (shift, cycles) => shift is null ? 0 : shift.Value.Time * cycles);
            SecondaryCycles = time.CombineLatest(Cycle, (time, cycle) => time / cycle).ToReadOnlyReactiveProperty();
        }

        public ReadOnlyReactiveProperty<Shift?> Shift => _shift.Value;

        public ReadOnlyReactiveProperty<float> Cycles => _cycles.InterpolatedValue;

        public ReadOnlyReactiveProperty<float> Height { get; }

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        public ReadOnlyReactiveProperty<float> Cycle { get; }

        public ReadOnlyReactiveProperty<float> SecondaryCycles { get; }

        public void Dispose()
        {
            _sync.Dispose();
        }

        public void Extrapolate()
        {
            _sync.Extrapolate();
        }

        public void Interpolate(float time)
        {
            _sync.Interpolate(time);
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            _sync.Read(ref reader, mode);
        }

        public void Reset()
        {
            _sync.Reset();
        }
    }
}
