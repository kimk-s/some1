using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using MemoryPack;
using R3;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectHitGroup : ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly ObjectHit[] _all;
        private readonly int[] _attributeValues;
        private int _lastToken;

        internal ObjectHitGroup(
            int count,
            ObjectEnergyGroup energies,
            ITime time)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            _all = Enumerable.Range(0, count).Select(_ => new ObjectHit(energies, time)).ToArray();
            _sync = new SyncArraySource(_all);

            _attributeValues = new int[EnumForUnity.GetValues<HitAttribute>().Length];
        }

        internal IReadOnlyList<IObjectHit> All => _all;

        internal bool CanAdd => Enabled;

        internal bool Enabled { get; set; }

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal bool Add(HitId id, int value, int rootId, float angle, BirthId? birthId)
        {
            if (!CanAdd)
            {
                return false;
            }

            if (id.IsUnique() && birthId is not null && ContainsUnique(birthId.Value))
            {
                return false;
            }

            GetProperOrOldest().Set(id, value, rootId, angle, birthId, GenerateToken());

            AddAttributeValue(id, value);

            return true;
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
            ClearAttributeValues();
        }

        internal void Update(float deltaSeconds)
        {
            if (!Enabled)
            {
                return;
            }

            foreach (var item in _all.AsSpan())
            {
                item.Update(deltaSeconds);
            }
        }

        internal int GetAttributeValue(HitAttribute attribute)
        {
            return _attributeValues[(int)attribute];
        }

        private void AddAttributeValue(HitId id, int value)
        {
            var arr = _attributeValues;
            int length = arr.Length;

            for (int i = 0; i < length; i++)
            {
                if (id.HasAttribute((HitAttribute)i))
                {
                    arr[i] += value;
                }
            }
        }

        internal void ClearAttributeValues()
        {
            if (!Enabled)
            {
                return;
            }

            var arr = _attributeValues;
            int length = arr.Length;
            for (int i = 0; i < length; i++)
            {
                arr[i] = 0;
            }
        }

        private int GenerateToken()
        {
            return ++_lastToken;
        }

        private ObjectHit GetProperOrOldest()
        {
            ObjectHit oldest = null!;

            foreach (var item in _all.AsSpan())
            {
                if (item.Id is null || item.Cycles.B > 0.8f)
                {
                    return item;
                }

                if (oldest is null || oldest.Token > item.Token)
                {
                    oldest = item;
                }
            }

            return oldest;
        }

        private bool ContainsUnique(BirthId birthId)
        {
            foreach (var item in _all.AsSpan())
            {
                if (item.Id?.IsUnique() == true && item.BirthId == birthId)
                {
                    return true;
                }
            }

            return false;
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
