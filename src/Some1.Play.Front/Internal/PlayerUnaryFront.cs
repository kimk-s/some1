using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerUnaryFront : IPlayerUnaryFront, ISyncDestination
    {
        private readonly NullablePackableParticleDestination<UnaryResult> _result = new();

        internal PlayerUnaryFront()
        {
        }

        public ReadOnlyReactiveProperty<UnaryResult?> Result => _result.Value;

        public ReadOnlyReactiveProperty<bool> IsDefault => _result.IsDefault;

        public void Dispose()
        {
            _result.Dispose();
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            _result.Read(ref reader, mode);
        }

        public void Reset()
        {
            _result.Reset();
        }
    }
}
