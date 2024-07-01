using System;
using System.Buffers;
using System.Collections.Generic;
using MemoryPack;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;
using Some1.Sync;
using R3;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectGiveStuff : IObjectGiveStuff, ISyncSource
    {
        private readonly UnmanagedParticleSource<int> _sync;
        private readonly ReactiveProperty<int> _number = new();
        private readonly HashSet<int> _givenObjectIds = new();
        private readonly IObjectProperties _properties;
        private readonly Space _space;
        private readonly Func<float> _getAliveCycles;
        private CharacterGiveStuffInfo? _info;

        public ObjectGiveStuff(IObjectProperties properties, Space space, Func<float> getAliveCycles)
        {
            _sync = _number.ToUnmanagedParticleSource();
            _properties = properties;
            _space = space;
            _getAliveCycles = getAliveCycles;
        }

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public int GivenCount => _givenObjectIds.Count;

        internal void Set(CharacterGiveStuffInfo? info)
        {
            _info = info;
        }

        internal void Update(ParallelToken parallelToken)
        {
            if (_info is null)
            {
                return;
            }

            if (_getAliveCycles() < 1)
            {
                return;
            }

            _number.Value += _space.AddGiveStuff(_info.Stuff, _properties.Area, _givenObjectIds, parallelToken);
        }

        internal void Reset()
        {
            _number.Value = 0;
            _givenObjectIds.Clear();
            _info = null;
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
            int? objectId = (writer.Options.ServiceProvider as ObjectIdServiceProvider)?.ObjectId;
            bool value = objectId is not null && _givenObjectIds.Contains(objectId.Value);

            if (value == default)
            {
                writer.WriteUnmanaged((byte)0);
            }
            else
            {
                writer.WriteUnmanaged((byte)1);
                writer.WriteUnmanaged(value);
            }
        }

        public void Dispose()
        {
            _sync.Dispose();
        }
    }
}
