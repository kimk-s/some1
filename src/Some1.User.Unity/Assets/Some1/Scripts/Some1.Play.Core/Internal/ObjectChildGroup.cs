using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectChildGroup
    {
        private readonly Object _self;
        private readonly IObjectFactory _factory;
        private readonly ITime _time;
        private readonly List<ObjectChild> _all = new();

        private const double ShrinkInterval = 2;
        private double _shrinkTime;

        internal ObjectChildGroup(Object self, IObjectFactory factory, ITime time)
        {
            _self = self;
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _time = time;
        }

        internal IReadOnlyList<ObjectChild> All => _all;

        internal bool CanAdd => true;

        private BirthId BirthId => new(_self.Id, _time.FrameCount);

        internal bool Add(
            CharacterId characterId,
            SkinId? skinId,
            byte team,
            int power,
            Vector2 position,
            Aim aim,
            ParallelToken? parallelToken)
        {
            if (!CanAdd)
            {
                return false;
            }

            if (TryGet(out var c))
            {
                c.Set(
                    characterId,
                    skinId,
                    team,
                    power,
                    position,
                    aim,
                    BirthId,
                    parallelToken);
            }

            return true;
        }

        internal void Update(ParallelToken parallelToken)
        {
            Shrink(parallelToken);
        }

        private void Shrink(ParallelToken parallelToken)
        {
            if (_shrinkTime > _time.TotalSeconds)
            {
                return;
            }
            double shrinkTime = _shrinkTime;
            _shrinkTime = _time.TotalSeconds + ShrinkInterval;
            if (shrinkTime == 0)
            {
                return;
            }

            var area = Area.Rectangle(
                _self.Properties.Area.Position + PlayConst.ChildShrinkAreaPosition,
                PlayConst.ChildShrinkAreaSize);

            foreach (var item in CollectionsMarshal.AsSpan(_all))
            {
                if (item.Object.Character.Id.CurrentValue is null
                    || item.Object.Properties.Area.IntersectsWith(area)
                    || (_time.TotalSeconds - item.TotalSecondsSet) < PlayConst.ChildShrinkSeconds)
                {
                    continue;
                }

                item.Object.ResetOrReserve(parallelToken);
            }
        }

        internal void Reset()
        {
            _shrinkTime = 0;
        }

        private bool TryGet([MaybeNullWhen(false)] out ObjectChild child)
        {
            foreach (var item in CollectionsMarshal.AsSpan(_all))
            {
                if (item.TryTakeControlForSet())
                {
                    child = item;
                    return true;
                }
            }

            if (_all.Count < PlayConst.MaxChildCount)
            {
                child = new(_self, _factory, _time);
                _all.Add(child);
                return true;
            }

            child = null;
            return false;
        }
    }
}
