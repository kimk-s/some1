using System.Collections.Generic;
using System.Linq;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class ObjectTakeStuffGroupFront : IObjectTakeStuffGroupFront, ISyncDestination, ISyncPolatable
    {
        private readonly SyncArrayDestination _sync;
        private readonly ObjectTakeStuffFront[] _all;
        private readonly UnmanagedParticleDestination<int> _comboScore = new();
        private readonly FloatWaveDestination _comboCycles;

        public ObjectTakeStuffGroupFront(ISyncTime syncFrame)
        {
            _all = Enumerable.Range(0, PlayConst.TakeStuffCount)
                .Select(_ => new ObjectTakeStuffFront(syncFrame))
                .ToArray();

            _comboCycles = new(syncFrame);

            _sync = new SyncArrayDestination(
                new SyncArrayDestination(_all),
                _comboScore,
                _comboCycles);
        }

        public IReadOnlyList<IObjectTakeStuffFront> All => _all;

        public ReadOnlyReactiveProperty<int> ComboScore => _comboScore.Value;

        public ReadOnlyReactiveProperty<float> ComboCycles => _comboCycles.InterpolatedValue;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

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
