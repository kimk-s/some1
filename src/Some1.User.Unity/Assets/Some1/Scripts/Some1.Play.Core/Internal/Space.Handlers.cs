using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal partial class Space
    {
        internal Aim? GetStrikeScanAim(StrikeScan scan, ParallelToken parallelToken)
        {
            Aim? result = null;

            foreach (var item in GetObjects(scan.GetArea(), scan.Target, parallelToken))
            {
                if (!item.CanAddHit)
                {
                    continue;
                }

                if (item.Id == scan.SourceObjectId)
                {
                    continue;
                }

                var delta = item.Properties.Area.Position - scan.SourceArea.Position;
                if (result is not null && delta.Length() >= result.Value.Length)
                {
                    continue;
                }

                var strike = new Strike(
                    scan.SourceArea.AddPosition(new Aim(new Aim(delta).Rotation, scan.Offset).ToVector2()),
                    item.Properties.Area,
                    scan.BumpLevel);
                var aim = GetStrikeAim(strike);
                if (aim is null || (result is not null && aim.Value.Length >= result.Value.Length))
                {
                    continue;
                }

                result = aim;
            };

            return result;
        }

        private Aim? GetStrikeAim(Strike strike)
            => GetStrikeAim(strike, StrikeZone.Center)
            ?? GetStrikeAim(strike, StrikeZone.Left)
            ?? GetStrikeAim(strike, StrikeZone.Right)
            ?? GetStrikeAim(strike, StrikeZone.Random);

        private Aim? GetStrikeAim(Strike strike, StrikeZone zone)
            => CheckStrike(strike, zone) ? new(strike.GetDelta(zone)) : null;

        private bool CheckStrike(Strike strike, StrikeZone zone)
        {
            if (strike.Source.Size.Width <= 0
                || strike.BumpLevel is null
                || strike.GetDelta(zone) == Vector2.Zero)
            {
                return true;
            }

            float length = strike.GetDelta(zone).Length();
            int count = length > MaxCheckInterval ? (int)Math.Ceiling(length / MaxCheckInterval) : 1;
            var interval = strike.GetDelta(zone) / count;
            var tiles = Rectangle.Empty;

            for (int i = 1; i <= count; i++)
            {
                var item = strike.Source.AddPosition(interval * i);

                if (item.IntersectsWith(strike.Destination))
                {
                    return true;
                }

                var t = GetTileIds(item);
                if (tiles == t)
                {
                    continue;
                }
                tiles = t;

                for (int y = tiles.Top; y < tiles.Bottom; y++)
                {
                    for (int x = tiles.Left; x < tiles.Right; x++)
                    {
                        //var tileArea = Area.Rectangle(new PointF(x, y), PlayConstants.TileSize);

                        //if (!tileArea.IntersectsWith(source))
                        //{
                        //    continue;
                        //}

                        if (Bump(new(x, y), strike.BumpLevel.Value))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        internal Aim? GetWalkScanAim(WalkScan scan, ParallelToken parallelToken)
        {
            Aim? result = null;
            using var pathfinder = GetPathfinder(scan.GetArea());

            foreach (var item in GetObjects(scan.GetArea(), scan.Target, parallelToken))
            {
                if (!item.CanAddHit)
                {
                    continue;
                }

                if (item.Properties.Area.IntersectsWith(scan.Source))
                {
                    return null;
                }

                var delta = item.Properties.Area.Position - scan.Source.Position;
                if (result is not null && delta.Length() >= result.Value.Length)
                {
                    continue;
                }

                var move = new Move(
                    scan.Source,
                    item.Properties.Area.Position - scan.Source.Position,
                    scan.BumpLevel,
                    true);
                var aim = GetStraightMoveAim(move) ?? pathfinder.Find(move);
                if (aim is null || (result is not null && aim.Value.Length >= result.Value.Length))
                {
                    continue;
                }

                result = aim;
            }

            return result;
        }

        private Aim? GetStraightMoveAim(Move move) => CheckMove(move) ? new(move.Delta) : null;

        internal void AddLike(
            Like like,
            int exceptObjectId,
            TeamTarget teamTarget,
            Area area,
            ParallelToken parallelToken)
        {
            foreach (var item in GetObjects(area, new(CharacterTypeTarget.Player, teamTarget), parallelToken))
            {
                if (item.Id != exceptObjectId)
                {
                    item.Messages.AddLike(like, parallelToken);
                }
            }
        }

        internal int AddGiveStuff(Stuff stuff, Area area, HashSet<int> giveContext, ParallelToken parallelToken)
        {
            int givenCount = 0;

            foreach (var item in GetObjects(area, new(CharacterTypeTarget.Player, TeamTarget.All), parallelToken))
            {
                switch (stuff.Type)
                {
                    case StuffType.Booster:
                        if (!item.CanAddBooster)
                        {
                            continue;
                        }
                        break;
                    case StuffType.Specialty:
                        if (!item.CanAddSpecialty)
                        {
                            continue;
                        }
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                if (giveContext.Contains(item.Id))
                {
                    continue;
                }

                switch (stuff.Type)
                {
                    case StuffType.Booster:
                        {
                            var booster = (BoosterStuff)stuff;
                            item.Messages.AddBooster(booster.Id, booster.Number, parallelToken);
                        }
                        break;
                    case StuffType.Specialty:
                        {
                            var specialty = (SpecialtyStuff)stuff;
                            item.Messages.AddSpecialty(specialty.Id, specialty.Number, parallelToken);
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }

                givenCount++;
                giveContext.Add(item.Id);
            }

            return givenCount;
        }
    }
}
