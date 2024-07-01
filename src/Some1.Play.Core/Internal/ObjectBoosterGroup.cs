using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;
using R3;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectBoosterGroup : ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly Dictionary<BoosterId, ObjectBooster> _all;
        private readonly ObjectTakeStuffGroup _stuffs;
        private readonly ObjectProperties _properties;

        internal ObjectBoosterGroup(
            BoosterInfoGroup infos,
            ObjectTakeStuffGroup takeStuffs,
            ObjectStatGroup stats,
            ObjectProperties properties,
            ITime time)
        {
            if (infos is null)
            {
                throw new ArgumentNullException(nameof(infos));
            }

            _all = infos.ById.Values
                .Select(x => new ObjectBooster(x, stats, time))
                .ToDictionary(x => x.Id);
            All = _all.ToDictionary(x => x.Key, x => (IObjectBooster)x.Value);
            _stuffs = takeStuffs;
            _properties = properties;
            _sync = new SyncArraySource(_all.Values.OrderBy(x => x.Id));
        }

        public IReadOnlyDictionary<BoosterId, IObjectBooster> All { get; }

        internal bool CanAdd => Enabled;

        internal bool Enabled { get; set; }

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal bool Add(BoosterId id, int number, bool ignoreCheckCanAdd = false)
        {
            if (!ignoreCheckCanAdd & !CanAdd)
            {
                return false;
            }

            var booster = _all[id];
            booster.Add(number);

            if (booster.Id == BoosterId.Power)
            {
                _properties.Power = booster.Number;
            }

            _stuffs.Add(new BoosterStuff(id, 1));

            return true;
        }

        internal void Update(float deltaSeconds)
        {
            if (!Enabled)
            {
                return;
            }

            foreach (var item in _all.Values)
            {
                item.Update(deltaSeconds);
            }
        }

        internal void Reset()
        {
            Enabled = false;

            foreach (var item in _all.Values)
            {
                item.Reset();
            }
        }

        internal void Stop()
        {
            if (!Enabled)
            {
                return;
            }

            foreach (var item in _all.Values)
            {
                item.Stop();
            }
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
