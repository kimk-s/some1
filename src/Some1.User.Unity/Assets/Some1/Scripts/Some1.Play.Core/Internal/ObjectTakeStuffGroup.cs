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
    internal sealed class ObjectTakeStuffGroup : IObjectTakeTakeStuffGroup, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly ObjectTakeStuff[] _all;
        private readonly ReactiveProperty<int> _comboScore = new();
        private readonly ReactiveProperty<FloatWave> _comboCycles = new();
        private int _lastToken;

        public ObjectTakeStuffGroup(int count, ITime time)
        {
            _all = Enumerable.Range(0, count).Select(_ => new ObjectTakeStuff(time)).ToArray();

            _sync = new SyncArraySource(
                new SyncArraySource(_all),
                _comboScore.ToUnmanagedParticleSource(),
                _comboCycles.ToWaveSource(time));
        }

        public event EventHandler<Stuff>? Added;

        public IReadOnlyList<IObjectTakeStuff> All => _all;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        public ReadOnlyReactiveProperty<int> ComboScore => _comboScore;

        public float ComboCycles => _comboCycles.Value.B;

        internal bool Enabled { get; set; }

        internal void Add(Stuff stuff)
        {
            if (!Enabled)
            {
                return;
            }

            GetOldest().Set(stuff, GenerateToken());

            _comboScore.Value += stuff.Score;
            _comboCycles.Value = FloatWave.Zero;

            Added?.Invoke(this, stuff);
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

            if (ComboScore.CurrentValue > 0)
            {
                _comboCycles.Value = _comboCycles.Value.Flow(deltaSeconds / PlayConst.TakeStuffComboTime);
                if (ComboCycles > 1)
                {
                    ResetCombo();
                }
            }
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

            foreach (var item in _all)
            {
                item.Reset();
            }
            _lastToken = 0;
            ResetCombo();
        }

        private void ResetCombo()
        {
            _comboScore.Value = 0;
            _comboCycles.Value = FloatWave.Zero;
        }

        private int GenerateToken()
        {
            return ++_lastToken;
        }

        private ObjectTakeStuff GetOldest()
        {
            ObjectTakeStuff result = null!;
            foreach (var item in _all)
            {
                if (result is null || result.Cycles.B > item.Cycles.B)
                {
                    result = item;
                }
            }
            return result;
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
