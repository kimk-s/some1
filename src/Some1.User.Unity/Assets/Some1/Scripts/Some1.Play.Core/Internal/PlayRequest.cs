using System;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;
using R3;

namespace Some1.Play.Core.Internal
{
    internal sealed class PlayRequest : ISyncReadable, IDisposable
    {
        private readonly SyncArrayDestination _sync;
        private readonly NullablePackableParticleDestination<Unary> _unary = new();
        private readonly NullableUnmanagedParticleDestination<Cast> _cast = new();
        private readonly NullableUnmanagedParticleDestination<Walk> _walk = new();

        public PlayRequest()
        {
            _sync = new SyncArrayDestination(
                _unary,
                _cast,
                _walk);
        }

        public ReadOnlyReactiveProperty<Unary?> Unary => _unary.Value;

        public ReadOnlyReactiveProperty<Cast?> Cast => _cast.Value;

        public ReadOnlyReactiveProperty<Walk?> Walk => _walk.Value;

        public void Dispose()
        {
            _sync.Dispose();
        }

        public void Reset()
        {
            _sync.Reset();
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            reader.ReadDestinationDirtySafely(_sync, mode);
        }
    }
}
