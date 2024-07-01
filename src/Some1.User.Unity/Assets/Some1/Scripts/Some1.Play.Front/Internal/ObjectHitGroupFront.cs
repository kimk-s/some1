using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MemoryPack;
using R3;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;

namespace Some1.Play.Front.Internal
{
    internal sealed class ObjectHitGroupFront : IObjectHitGroupFront, ISyncDestination, ISyncPolatable
    {
        private readonly SyncArrayDestination _sync;
        private readonly ObjectHitFront[] _all;
        private readonly ReactiveProperty<Vector2> _damagePush = new();
        private readonly ReactiveProperty<float?> _damageMinimumCycles = new();

        public ObjectHitGroupFront(ISyncTime syncFrame, ReadOnlyReactiveProperty<int> objectId, IPlayerObjectFront playerObject)
        {
            _all = Enumerable.Range(0, PlayConst.HitCount)
                .Select(_ => new ObjectHitFront(syncFrame, objectId, playerObject))
                .ToArray();

            foreach (var item in _all)
            {
                item.Cycles.Subscribe(_ => UpdateDamage());
            }

            _sync = new(_all);
        }

        public IReadOnlyList<IObjectHitFront> All => _all;

        public ReadOnlyReactiveProperty<Vector2> DamagePush => _damagePush;

        public ReadOnlyReactiveProperty<float?> DamageMinimumCycles => _damageMinimumCycles;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        public void Dispose()
        {
            _sync.Dispose();
            _damagePush.Dispose();
            _damageMinimumCycles.Dispose();
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

        private void UpdateDamage()
        {
            const float Time = PlayConst.HitShortSeconds;
            const float HalfTime = Time * 0.5f;
            const float OneDividedByHalfTime = 1 / HalfTime;
            const float Length = 0.1f;
            var push = Vector2.Zero;
            float? minimumCycles = null;

            foreach (var item in _all)
            {
                var hit = item.Hit?.CurrentValue;
                var cycles = item.Cycles.CurrentValue;

                if (hit is null || !hit.Value.Id.IsDamage() || PlayConst.IsHitAngleQuiet(hit.Value.Angle) || cycles > Time)
                {
                    continue;
                }

                var temp = Vector2Helper.Normalize(hit.Value.Angle) * Length;
                if (cycles <= HalfTime)
                {
                    temp *= cycles * OneDividedByHalfTime;
                }
                else
                {
                    temp *= 1 - (cycles - HalfTime) * OneDividedByHalfTime;
                }

                push += temp;

                if (minimumCycles is null || minimumCycles.Value > cycles)
                {
                    minimumCycles = cycles;
                }
            }

            if (push.Length() > Length)
            {
                push = Vector2.Normalize(push) * Length;
            }

            _damagePush.Value = push;
            _damageMinimumCycles.Value = minimumCycles;
        }
    }
}
