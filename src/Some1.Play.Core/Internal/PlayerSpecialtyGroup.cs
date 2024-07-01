using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using MemoryPack;
using R3;
using Some1.Play.Data;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class PlayerSpecialtyGroup : ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly Dictionary<SpecialtyId, PlayerSpecialty> _all;
        private readonly PlayerTitle _title;
        private readonly ITime _time;
        private readonly SpecialtyGroupData _save;

        internal PlayerSpecialtyGroup(SpecialtyInfoGroup infos, PlayerTitle title, ITime time)
        {
            _all = infos.ById.ToDictionary(
                x => x.Key,
                x => new PlayerSpecialty(x.Value));
            All = _all.ToDictionary(
                x => x.Key,
                x => (IPlayerSpecialty)x.Value);
            _title = title;
            _time = time;
            _sync = new SyncArraySource(_all.Values.OrderBy(x => x.Id));
            _save = new()
            {
                Items = new SpecialtyData[_all.Count]
            };
        }

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal IReadOnlyDictionary<SpecialtyId, IPlayerSpecialty> All { get; }

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

        internal void Load(SpecialtyGroupData? data)
        {
            if (data is null)
            {
                return;
            }

            foreach (var x in data.Items)
            {
                Add((SpecialtyId)x.Id, x.Number, x.NumberUtc);
            }
        }

        internal SpecialtyGroupData Save()
        {
            var save = _save;

            int i = 0;
            foreach (var item in _all.Values)
            {
                save.Items[i++] = item.Save();
            }

            return save;
        }

        internal void Add(SpecialtyId id, int value)
        {
            Add(id, value, _time.UtcNow);
        }

        private void Add(SpecialtyId id, int value, DateTime utc)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            if (!_all.TryGetValue(id, out var item))
            {
                return;
            }

            int star = item.Star;

            item.AddNumber(value, utc);

            int starDiff = item.Star - star;
            if (starDiff < 0)
            {
                throw new InvalidOperationException();
            }
            else if (starDiff > 0)
            {
                _title.AddStar(starDiff);
            }
        }

        internal void Reset()
        {
            foreach (var item in _all.Values)
            {
                item.Reset();
            }
        }
    }
}
