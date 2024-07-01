using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed partial class Space : ISpace
    {
        private const float MaxCheckInterval = 0.1f;

        private static readonly ThreadLocal<ObjectSetState> s_objectSetState = new(() => new());
        private static readonly ThreadLocal<PathfinderState> s_pathfinderState = new(() => new());

        private readonly ObjectBlock[,] _blocks;
        private readonly Tile[,] _tiles;

        internal Space(SpaceInfo info, ParallelOptions parallelOptions, ITime time)
        {
            if (info.SpaceTiles.Width % info.BlockTiles != 0 || info.SpaceTiles.Height % info.BlockTiles != 0)
            {
                throw new InvalidOperationException();
            }

            Info = info;
            Area = Area.Rectangle(PointF.Empty, info.SpaceTiles);

            _tiles = new Tile[(int)Area.Size.Height, (int)Area.Size.Width];

            var blockCount = new Size((int)Area.Size.Width / info.BlockTiles, (int)Area.Size.Height / info.BlockTiles);
            _blocks = new ObjectBlock[blockCount.Height, blockCount.Width];
            for (var y = 0; y < _blocks.GetLength(0); y++)
            {
                for (var x = 0; x < _blocks.GetLength(1); x++)
                {
                    _blocks[y, x] = new(
                        new(x, y),
                        Area.Rectangle(
                            new PointF(x * info.BlockTiles, y * info.BlockTiles),
                            info.BlockTiles),
                        parallelOptions,
                        time,
                        new(
                            _tiles,
                            new(
                                x * info.BlockTiles,
                                y * info.BlockTiles,
                                info.BlockTiles,
                                info.BlockTiles)));
                }
            }

            Blocks = _blocks.Cast<IBlock<IObject>>();
        }

        public SpaceInfo Info { get; }

        public Area Area { get; }

        public int BlockCount => _blocks.Length;

        public IEnumerable<IBlock<IObject>> Blocks { get; }

        IBlock<IObject> ISpace.GetBlock(BlockId id) => GetBlock(id);

        IBlock<IObject>[] ISpace.GetBlocks(Area area)
        {
            var results = new List<ObjectBlock>();
            GetBlocks(area, results);
            return results.ToArray();
        }

        IObject[] ISpace.GetObjects(Area area)
        {
#pragma warning disable CS0612 // Type or member is obsolete
            using var objects = GetObjectSet(area);
#pragma warning restore CS0612 // Type or member is obsolete
            return objects.ToArray();
        }

        internal void SetObjectPosition(Object obj, Vector2Wave position, ParallelToken? parallelToken)
        {
            if (obj.Transform.Position.CurrentValue == position)
            {
                obj.SetTransformPositionBySpace(position);
                return;
            }

            if (obj.Character.Info?.BumpLevel is not null)
            {
                if (obj.BlockIds != default)
                {
                    throw new InvalidOperationException();
                }

                Debug.Assert(obj.Properties.Area.Size.Width == obj.Properties.Area.Size.Height
                    && obj.Properties.Area.Size.Width == (int)obj.Properties.Area.Size.Width);

                var area = obj.Properties.Area;
                area.Position = position.B;
                area.RoundLocation();
                position = new(area.Position);
            }

            var oldIds = obj.BlockIds;
            obj.SetTransformPositionBySpace(position);
            var newIds = GetBlockIds(obj.Properties.Area);

            if (oldIds == newIds)
            {
                return;
            }
            obj.BlockIds = newIds;

            foreach (var id in oldIds)
            {
                if (!newIds.Contains(id))
                {
                    GetBlock(id).Remove(obj, parallelToken);
                }
            }

            foreach (var id in newIds)
            {
                if (!oldIds.Contains(id))
                {
                    GetBlock(id).Add(obj, parallelToken);
                }
            }
        }

        internal void UpdateNonLeaderObjects(LeaderToken leaderToken, ParallelToken parallelToken)
        {
            if (leaderToken is null)
            {
                throw new ArgumentNullException(nameof(leaderToken));
            }

            if (parallelToken is null)
            {
                throw new ArgumentNullException(nameof(parallelToken));
            }

            foreach (var id in GetBlockIds(leaderToken.Area))
            {
                foreach (var obj in CollectionsMarshal.AsSpan(GetBlock(id).Items))
                {
                    if (obj.Value.Properties.Area.IntersectsWith(leaderToken.Area))
                    {
                        _ = obj.Value.Update(false, parallelToken);
                    }
                }
            }
        }

        internal void UpdateBlocks(LeaderToken leaderToken)
        {
            foreach (var id in GetBlockIds(leaderToken.Area))
            {
                _ = GetBlock(id).TryUpdate();
            }
        }

        internal bool Bump(Point point, BumpLevel level)
        {
            var tiles = _tiles;

            if (point.X < 0
                || point.X >= tiles.GetLength(1)
                || point.Y < 0
                || point.Y >= tiles.GetLength(0))
            {
                return true;
            }

            var tileLevel = tiles[point.Y, point.X].BumpLevel;

            return tileLevel is not null && tileLevel.Value >= level;
        }

        internal bool CheckMove(Move move)
        {
            if (!Area.Contains(move.Destination))
            {
                return false;
            }

            if (move.Source.Size.Width <= 0
                || move.BumpLevel is null
                || move.Delta == Vector2.Zero)
            {
                return true;
            }

            float length = move.Delta.Length();
            int count = length > MaxCheckInterval ? (int)Math.Ceiling(length / MaxCheckInterval) : 1;
            var interval = move.Delta / count;
            var tiles = Rectangle.Empty;

            for (int i = 1; i <= count; i++)
            {
                var item = move.Source.AddPosition(interval * i);
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
                        var tileArea = Area.Rectangle(new PointF(x, y), 1);

                        //if (!tileArea.IntersectsWith(item))
                        //{
                        //    continue;
                        //}

                        if (move.Walk && tileArea.IntersectsWith(move.Source))
                        {
                            continue;
                        }

                        if (Bump(new(x, y), move.BumpLevel.Value))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private void GetBlocks(Area area, List<ObjectBlock> results)
        {
            foreach (var item in GetBlockIds(area))
            {
                results.Add(GetBlock(item));
            }
        }

        private BlockIdGroup GetBlockIds(Area area)
        {
            area.Intersect(Area);

            return Rectangle.FromLTRB(
                (int)MathF.Floor(area.Left / Info.BlockTiles),
                (int)MathF.Floor(area.Top / Info.BlockTiles),
                (int)MathF.Ceiling(area.Right / Info.BlockTiles),
                (int)MathF.Ceiling(area.Bottom / Info.BlockTiles));
        }

        private Rectangle GetTileIds(Area area)
        {
            area.Intersect(Area);

            return Rectangle.FromLTRB(
                (int)MathF.Floor(area.Left),
                (int)MathF.Floor(area.Top),
                (int)MathF.Ceiling(area.Right),
                (int)MathF.Ceiling(area.Bottom));
        }

        private ObjectBlock GetBlock(BlockId id) => _blocks[id.Y, id.X];
    }
}
