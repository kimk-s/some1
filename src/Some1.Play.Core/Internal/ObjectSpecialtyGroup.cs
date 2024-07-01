using System;
using System.Collections.Generic;
using System.Linq;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectSpecialtyGroup
    {
        private readonly ObjectSpecialty[] _all;
        private readonly ObjectTakeStuffGroup _stuffs;
        private int _lastToken;

        internal ObjectSpecialtyGroup(int count, ObjectTakeStuffGroup takeStuffs)
        {
            _all = Enumerable.Range(0, count).Select(x => new ObjectSpecialty(x)).ToArray();
            _stuffs = takeStuffs;
        }

        public IReadOnlyList<IObjectSpecialty> All => _all;

        internal bool CanAdd => Enabled;

        internal bool Enabled { get; set; }

        internal int PinnedCount
        {
            get
            {
                int count = 0;
                foreach (var item in _all.AsSpan())
                {
                    if (item.IsPinned.CurrentValue)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        internal bool Add(SpecialtyId id, int number)
        {
            if (!CanAdd)
            {
                return false;
            }

            var item = Get(id) ?? GetNotPinnedAndOldest();

            item?.Add(id, number, GenerateToken());

            _stuffs.Add(new SpecialtyStuff(id, number));

            return true;
        }

        internal void Pin(SpecialtyId id, bool value)
        {
            if (!Enabled)
            {
                return;
            }

            if (value && PinnedCount >= PlayConst.SpecialtyMaxPinCount)
            {
                return;
            }

            var item = Get(id);

            item?.Pin(value, GenerateToken());
        }

        internal void Reset()
        {
            if (!Enabled)
            {
                return;
            }

            Stop();
            Enabled = false;
        }

        internal void Stop()
        {
            if (!Enabled)
            {
                return;
            }

            foreach (var item in _all.AsSpan())
            {
                item.Reset();
            }
            _lastToken = 0;
        }

        private int GenerateToken()
        {
            return ++_lastToken;
        }

        private ObjectSpecialty? Get(SpecialtyId id)
        {
            foreach (var item in _all.AsSpan())
            {
                if (item.Id.CurrentValue == id)
                {
                    return item;
                }
            }
            return null;
        }

        private ObjectSpecialty? GetNotPinnedAndOldest()
        {
            ObjectSpecialty? oldest = null;

            foreach (var item in _all.AsSpan())
            {
                if (!item.IsPinned.CurrentValue && (oldest is null || oldest.Token.CurrentValue > item.Token.CurrentValue))
                {
                    oldest = item;
                }
            }

            return oldest;
        }
    }
}
