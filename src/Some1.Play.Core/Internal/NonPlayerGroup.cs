using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class NonPlayerGroup
    {
        private readonly SeasonInfoGroup _seasonInfos;
        private readonly IObjectFactory _objectFactory;
        private readonly IRegionGroup _regions;
        private readonly Space _space;
        private readonly ITime _time;

        internal NonPlayerGroup(
            NonPlayerGenerationInfoGroup generationInfos,
            NonPlayerSpawningInfoGroup spawningInfos,
            SeasonInfoGroup seasonInfos,
            IObjectFactory objectFactory,
            IRegionGroup regions,
            Space space,
            ITime time)
        {
            _seasonInfos = seasonInfos;
            _objectFactory = objectFactory;
            _regions = regions;
            _space = space;
            _time = time;
            All = Enumerable.Concat(Generate(generationInfos), Spawn(spawningInfos));
        }

        internal IEnumerable<INonPlayer> All { get; }

        private static IEnumerable<Area> GetSpots(NonPlayerRectTransform rectTransform, NonPlayerDensity density, Area area)
        {
            var rt = rectTransform.Stretch
                ? RectangleF.FromLTRB(
                    area.Left + area.Size.Width * rectTransform.AnchorMin.X + rectTransform.Position.X,
                    area.Top + area.Size.Height * rectTransform.AnchorMin.Y + rectTransform.Position.Y,
                    area.Left + area.Size.Width * rectTransform.AnchorMax.X - rectTransform.Size.X,
                    area.Top + area.Size.Height * rectTransform.AnchorMax.Y - rectTransform.Size.Y)
                : new(
                    (area.Location.ToVector2() + area.Size.ToVector2() * rectTransform.AnchorMin + rectTransform.Position).ToPointF(),
                    rectTransform.Size.ToSizeF());

            if (!area.Contains(rt) || rt.Size.Width < 0 || rt.Size.Height < 0)
            {
                throw new InvalidOperationException();
            }

            var count = density.CountCeiling
                ? Size.Ceiling(rt.Size * density.Value)
                : Size.Truncate(rt.Size * density.Value);
            var interval = (rt.Size.ToVector2() / new SizeF(count).ToVector2()).ToSizeF();

            for (int y = 0; y < count.Height; y++)
            {
                for (int x = 0; x < count.Width; x++)
                {
                    yield return density.Spread
                        ? Area.Rectangle(
                            rt.Location + (interval.ToVector2() * new Vector2(x, y)).ToSizeF(),
                            interval)
                        : rt;
                }
            }
        }

        private IEnumerable<INonPlayer> Generate(NonPlayerGenerationInfoGroup infos)
        {
            var sections = _regions.All.Values.SelectMany(x => x.Sections).ToArray();
            var results = new List<NonPlayer>[sections.Length];

            Parallel.For(0, sections.Length, index =>
            {
                var section = sections[index];
                if (!infos.ByRegionSectionId.TryGetValue(section.Id, out var info)) return;
                var result = results[index] = new();

                AddFences(info.FenceId, section.Area, result);
                AddLandmark(info.LandmarkId, section.Area, result);
                AddPieces(info.Barrier, section.Area, result);
                AddPieces(info.Water, section.Area, result);
            });

            return results.Where(x => x is not null).SelectMany(x => x);
        }

        private IEnumerable<INonPlayer> Spawn(NonPlayerSpawningInfoGroup infos)
        {
            var sections = _regions.All.Values.SelectMany(x => x.Sections).ToArray();
            var results = new List<NonPlayer>[sections.Length];

            Parallel.For(0, sections.Length, index =>
            {
                var section = sections[index];
                if (!infos.ByRegionSectionId.TryGetValue(section.Id, out var infoList)) return;
                var result = results[index] = new();

                foreach (var info in infoList)
                {
                    foreach (var spot in GetSpots(info.Core.RectTransform, info.Core.Density, section.Area))
                    {
                        result.Add(CreateNonPlayer(
                            info.Core.CharacterId,
                            info.Core.Timing,
                            spot,
                            info.Team));
                    }
                }
            });

            return results.Where(x => x is not null).SelectMany(x => x);
        }

        [Flags]
        private enum Entrance
        {
            None = 0,
            Left = 1,
            Top = 2,
            Right = 4,
            Bottom = 8,
            All = Left | Top | Right | Bottom,
        }

        private void AddFences(CharacterId characterId, Area area, List<NonPlayer> result)
        {
            var entrance = (area.Left == _space.Area.Left ? Entrance.None : Entrance.Left)
                | (area.Right == _space.Area.Right ? Entrance.None : Entrance.Right)
                | (area.Top == _space.Area.Top ? Entrance.None : Entrance.Top)
                | (area.Bottom == _space.Area.Bottom ? Entrance.None : Entrance.Bottom);

            foreach (var a in GetFenceAreas(area, entrance))
            {
                AddFence(characterId, a, result);
            }
        }

        private IEnumerable<Area> GetFenceAreas(Area area, Entrance entrance)
        {
            if (area.Size.Width < _space.Info.FenceTiles * 2 + _space.Info.EntranceTiles
                || area.Size.Height < _space.Info.FenceTiles * 2 + _space.Info.EntranceTiles)
            {
                throw new InvalidOperationException();
            }

            // Corner - left top
            yield return Area.Rectangle(area.Location, _space.Info.FenceTiles);

            // Corner - right top
            yield return Area.Rectangle(area.Location + new SizeF(area.Size.Width - _space.Info.FenceTiles, 0), _space.Info.FenceTiles);

            // Corner - left bottom
            yield return Area.Rectangle(area.Location + new SizeF(0, area.Size.Height - _space.Info.FenceTiles), _space.Info.FenceTiles);

            // Corner - right bottom
            yield return Area.Rectangle(area.Location + new SizeF(area.Size.Width - _space.Info.FenceTiles, area.Size.Height - _space.Info.FenceTiles), _space.Info.FenceTiles);

            // Edge - left
            if (entrance.HasFlag(Entrance.Left))
            {
                yield return Area.Rectangle(
                    area.Location + new SizeF(0, _space.Info.FenceTiles),
                    new SizeF(_space.Info.FenceTiles, (area.Size.Height - _space.Info.EntranceTiles) * 0.5f - _space.Info.FenceTiles));
                yield return Area.Rectangle(
                    area.Location + new SizeF(0, (area.Size.Height + _space.Info.EntranceTiles) * 0.5f),
                    new SizeF(_space.Info.FenceTiles, (area.Size.Height - _space.Info.EntranceTiles) * 0.5f - _space.Info.FenceTiles));
            }
            else
            {
                yield return Area.Rectangle(
                    area.Location + new SizeF(0, _space.Info.FenceTiles),
                    new SizeF(_space.Info.FenceTiles, area.Size.Height - _space.Info.FenceTiles * 2));
            }

            // Edge - top
            if (entrance.HasFlag(Entrance.Top))
            {
                yield return Area.Rectangle(
                    area.Location + new SizeF(_space.Info.FenceTiles, 0),
                    new SizeF((area.Size.Width - _space.Info.EntranceTiles) * 0.5f - _space.Info.FenceTiles, _space.Info.FenceTiles));
                yield return Area.Rectangle(
                    area.Location + new SizeF((area.Size.Width + _space.Info.EntranceTiles) * 0.5f, 0),
                    new SizeF((area.Size.Width - _space.Info.EntranceTiles) * 0.5f - _space.Info.FenceTiles, _space.Info.FenceTiles));
            }
            else
            {
                yield return Area.Rectangle(
                    area.Location + new SizeF(_space.Info.FenceTiles, 0),
                    new SizeF(area.Size.Width - _space.Info.FenceTiles * 2, _space.Info.FenceTiles));
            }
            
            // Edge - right
            if (entrance.HasFlag(Entrance.Right))
            {
                yield return Area.Rectangle(
                    area.Location + new SizeF(area.Size.Width - _space.Info.FenceTiles, _space.Info.FenceTiles),
                    new SizeF(_space.Info.FenceTiles, (area.Size.Height - _space.Info.EntranceTiles) * 0.5f - _space.Info.FenceTiles));
                yield return Area.Rectangle(
                    area.Location + new SizeF(area.Size.Width - _space.Info.FenceTiles, (area.Size.Height + _space.Info.EntranceTiles) * 0.5f),
                    new SizeF(_space.Info.FenceTiles, (area.Size.Height - _space.Info.EntranceTiles) * 0.5f - _space.Info.FenceTiles));
            }
            else
            {
                yield return Area.Rectangle(
                    area.Location + new SizeF(area.Size.Width - _space.Info.FenceTiles, _space.Info.FenceTiles),
                    new SizeF(_space.Info.FenceTiles, area.Size.Height - _space.Info.FenceTiles * 2));
            }
            
            // Edge - bottom
            if (entrance.HasFlag(Entrance.Bottom))
            {
                yield return Area.Rectangle(
                    area.Location + new SizeF(_space.Info.FenceTiles, area.Size.Height - _space.Info.FenceTiles),
                    new SizeF((area.Size.Width - _space.Info.EntranceTiles) * 0.5f - _space.Info.FenceTiles, _space.Info.FenceTiles));
                yield return Area.Rectangle(
                    area.Location + new SizeF((area.Size.Width + _space.Info.EntranceTiles) * 0.5f, area.Size.Height - _space.Info.FenceTiles),
                    new SizeF((area.Size.Width - _space.Info.EntranceTiles) * 0.5f - _space.Info.FenceTiles, _space.Info.FenceTiles));
            }
            else
            {
                yield return Area.Rectangle(
                    area.Location + new SizeF(_space.Info.FenceTiles, area.Size.Height - _space.Info.FenceTiles),
                    new SizeF(area.Size.Width - _space.Info.FenceTiles * 2, _space.Info.FenceTiles));
            }
        }

        private void AddFence(CharacterId characterId, Area area, List<NonPlayer> result)
        {
            if (area.Size.Width % _space.Info.BrickTiles != 0
                || area.Size.Height % _space.Info.BrickTiles != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(area));
            }

            int width = (int)area.Size.Width / _space.Info.BrickTiles;
            int height = (int)area.Size.Height / _space.Info.BrickTiles;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    result.Add(CreateNonPlayer(
                        characterId,
                        default,
                        Area.Rectangle(
                            area.Location.ToVector2() + new Vector2(x + 0.5f, y + 0.5f) * _space.Info.BrickTiles,
                            0)));
                }
            }
        }

        private void AddLandmark(CharacterId characterId, Area area, List<NonPlayer> result)
        {
            result.Add(CreateNonPlayer(
                characterId,
                default,
                Area.Rectangle(area.Position, 0)));
        }

        private void AddPieces(NonPlayerSpawningCoreInfo info, Area area, List<NonPlayer> result)
        {
            AddPieces(info.CharacterId, info.Timing, info.RectTransform, info.Density, area, result);
        }

        private void AddPieces(
            CharacterId characterId,
            NonPlayerTiming timing,
            NonPlayerRectTransform rectTransform,
            NonPlayerDensity density,
            Area area,
            List<NonPlayer> result)
        {
            foreach (var spot in GetSpots(rectTransform, density, area))
            {
                var position = spot.Location.ToVector2() + spot.Size.ToVector2() * new Vector2(RandomForUnity.NextSingle(), RandomForUnity.NextSingle());
                var piece = new Piece(
                    (PieceType)RandomForUnity.Next((int)PieceType._Count),
                    (PieceRotation)RandomForUnity.Next((int)PieceRotation._Count));
                AddPiece(characterId, timing, position, piece, result);
            }
        }

        private void AddPiece(
            CharacterId characterId,
            NonPlayerTiming timing,
            Vector2 position,
            Piece piece,
            List<NonPlayer> result)
        {
            var area = Area.Rectangle(position, GetPieceSize(piece));
            area.RoundLocation();
            var tile = Point.Truncate(area.Location);
            foreach (var item in GetPieceTiles(piece))
            {
                TryAddTile(characterId, timing, tile + item, result);
            }
        }

        private Size GetPieceSize(Piece piece)
        {
            switch (piece.type)
            {
                case PieceType.I:
                    {
                        switch (piece.rotation)
                        {
                            case PieceRotation.One:
                                return new(4, 1);
                            case PieceRotation.Two:
                                return new(1, 4);
                            case PieceRotation.Three:
                                return new(4, 1);
                            case PieceRotation.Four:
                                return new(1, 4);
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                case PieceType.J:
                    {
                        switch (piece.rotation)
                        {
                            case PieceRotation.One:
                                return new(3, 2);
                            case PieceRotation.Two:
                                return new(2, 3);
                            case PieceRotation.Three:
                                return new(3, 2);
                            case PieceRotation.Four:
                                return new(2, 3);
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                case PieceType.L:
                    switch (piece.rotation)
                    {
                        case PieceRotation.One:
                            return new(3, 2);
                        case PieceRotation.Two:
                            return new(2, 3);
                        case PieceRotation.Three:
                            return new(3, 2);
                        case PieceRotation.Four:
                            return new(2, 3);
                        default:
                            throw new InvalidOperationException();
                    }
                case PieceType.O:
                    switch (piece.rotation)
                    {
                        case PieceRotation.One:
                            return new(2, 2);
                        case PieceRotation.Two:
                            return new(2, 2);
                        case PieceRotation.Three:
                            return new(2, 2);
                        case PieceRotation.Four:
                            return new(2, 2);
                        default:
                            throw new InvalidOperationException();
                    }
                case PieceType.S:
                    switch (piece.rotation)
                    {
                        case PieceRotation.One:
                            return new(3, 2);
                        case PieceRotation.Two:
                            return new(2, 3);
                        case PieceRotation.Three:
                            return new(3, 2);
                        case PieceRotation.Four:
                            return new(2, 3);
                        default:
                            throw new InvalidOperationException();
                    }
                case PieceType.T:
                    switch (piece.rotation)
                    {
                        case PieceRotation.One:
                            return new(3, 2);
                        case PieceRotation.Two:
                            return new(2, 3);
                        case PieceRotation.Three:
                            return new(3, 2);
                        case PieceRotation.Four:
                            return new(2, 3);
                        default:
                            throw new InvalidOperationException();
                    }
                case PieceType.Z:
                    switch (piece.rotation)
                    {
                        case PieceRotation.One:
                            return new(3, 2);
                        case PieceRotation.Two:
                            return new(2, 3);
                        case PieceRotation.Three:
                            return new(3, 2);
                        case PieceRotation.Four:
                            return new(2, 3);
                        default:
                            throw new InvalidOperationException();
                    }
                default:
                    throw new InvalidOperationException();
            }
        }

        private IEnumerable<Size> GetPieceTiles(Piece piece)
        {
            switch (piece.type)
            {
                case PieceType.I:
                    switch (piece.rotation)
                    {
                        case PieceRotation.One:
                            yield return new(0, 0);
                            yield return new(1, 0);
                            yield return new(2, 0);
                            yield return new(3, 0);
                            break;
                        case PieceRotation.Two:
                            yield return new(0, 0);
                            yield return new(0, 1);
                            yield return new(0, 2);
                            yield return new(0, 3);
                            break;
                        case PieceRotation.Three:
                            yield return new(0, 0);
                            yield return new(1, 0);
                            yield return new(2, 0);
                            yield return new(3, 0);
                            break;
                        case PieceRotation.Four:
                            yield return new(0, 0);
                            yield return new(0, 1);
                            yield return new(0, 2);
                            yield return new(0, 3);
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                    break;
                case PieceType.J:
                    switch (piece.rotation)
                    {
                        case PieceRotation.One:
                            yield return new(0, 0);
                            yield return new(-1, 0);
                            yield return new(-1, 1);
                            yield return new(-1, 2);
                            break;
                        case PieceRotation.Two:
                            yield return new(0, 0);
                            yield return new(1, 0);
                            yield return new(0, 1);
                            yield return new(0, 2);
                            break;
                        case PieceRotation.Three:
                            yield return new(0, 0);
                            yield return new(1, 0);
                            yield return new(2, 0);
                            yield return new(2, 1);
                            break;
                        case PieceRotation.Four:
                            yield return new(1, 0);
                            yield return new(1, 1);
                            yield return new(0, 2);
                            yield return new(1, 2);
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                    break;
                case PieceType.L:
                    switch (piece.rotation)
                    {
                        case PieceRotation.One:
                            yield return new(2, 0);
                            yield return new(0, 1);
                            yield return new(1, 1);
                            yield return new(2, 1);
                            break;
                        case PieceRotation.Two:
                            yield return new(0, 0);
                            yield return new(0, 1);
                            yield return new(0, 2);
                            yield return new(1, 2);
                            break;
                        case PieceRotation.Three:
                            yield return new(0, 0);
                            yield return new(1, 0);
                            yield return new(2, 0);
                            yield return new(0, 1);
                            break;
                        case PieceRotation.Four:
                            yield return new(0, 0);
                            yield return new(1, 0);
                            yield return new(1, 1);
                            yield return new(1, 2);
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                    break;
                case PieceType.O:
                    switch (piece.rotation)
                    {
                        case PieceRotation.One:
                            yield return new(0, 0);
                            yield return new(1, 0);
                            yield return new(0, 1);
                            yield return new(1, 1);
                            break;
                        case PieceRotation.Two:
                            yield return new(0, 0);
                            yield return new(1, 0);
                            yield return new(0, 1);
                            yield return new(1, 1);
                            break;
                        case PieceRotation.Three:
                            yield return new(0, 0);
                            yield return new(1, 0);
                            yield return new(0, 1);
                            yield return new(1, 1);
                            break;
                        case PieceRotation.Four:
                            yield return new(0, 0);
                            yield return new(1, 0);
                            yield return new(0, 1);
                            yield return new(1, 1);
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                    break;
                case PieceType.S:
                    switch (piece.rotation)
                    {
                        case PieceRotation.One:
                            yield return new(1, 0);
                            yield return new(2, 0);
                            yield return new(0, 1);
                            yield return new(1, 1);
                            break;
                        case PieceRotation.Two:
                            yield return new(0, 0);
                            yield return new(0, 1);
                            yield return new(1, 1);
                            yield return new(1, 2);
                            break;
                        case PieceRotation.Three:
                            yield return new(1, 0);
                            yield return new(2, 0);
                            yield return new(0, 1);
                            yield return new(1, 1);
                            break;
                        case PieceRotation.Four:
                            yield return new(0, 0);
                            yield return new(0, 1);
                            yield return new(1, 1);
                            yield return new(1, 2);
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                    break;
                case PieceType.T:
                    switch (piece.rotation)
                    {
                        case PieceRotation.One:
                            yield return new(1, 0);
                            yield return new(0, 1);
                            yield return new(1, 1);
                            yield return new(2, 1);
                            break;
                        case PieceRotation.Two:
                            yield return new(0, 0);
                            yield return new(0, 1);
                            yield return new(1, 1);
                            yield return new(0, 2);
                            break;
                        case PieceRotation.Three:
                            yield return new(0, 0);
                            yield return new(1, 0);
                            yield return new(2, 0);
                            yield return new(1, 1);
                            break;
                        case PieceRotation.Four:
                            yield return new(1, 0);
                            yield return new(0, 1);
                            yield return new(1, 1);
                            yield return new(1, 2);
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                    break;
                case PieceType.Z:
                    switch (piece.rotation)
                    {
                        case PieceRotation.One:
                            yield return new(0, 0);
                            yield return new(1, 0);
                            yield return new(1, 1);
                            yield return new(2, 1);
                            break;
                        case PieceRotation.Two:
                            yield return new(1, 0);
                            yield return new(0, 1);
                            yield return new(1, 1);
                            yield return new(0, 2);
                            break;
                        case PieceRotation.Three:
                            yield return new(0, 0);
                            yield return new(1, 0);
                            yield return new(1, 1);
                            yield return new(2, 1);
                            break;
                        case PieceRotation.Four:
                            yield return new(1, 0);
                            yield return new(0, 1);
                            yield return new(1, 1);
                            yield return new(0, 2);
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private bool TryAddTile(
            CharacterId characterId,
            NonPlayerTiming timing,
            Point tile,
            List<NonPlayer> result)
        {
            if (_space.Bump(tile, BumpLevel.Low))
            {
                return false;
            }

            result.Add(CreateNonPlayer(
                characterId,
                timing,
                Area.Rectangle(tile, 1)));

            return true;
        }

        private NonPlayer CreateNonPlayer(
            CharacterId characterId,
            NonPlayerTiming timing,
            Area spot,
            byte team = Team.Environment)
        {
            return new(
                characterId,
                timing,
                spot,
                team,
                _objectFactory,
                _time);
        }

        private readonly struct Piece
        {
            public readonly PieceType type;
            public readonly PieceRotation rotation;

            public Piece(PieceType type, PieceRotation rotation)
            {
                this.type = type;
                this.rotation = rotation;
            }
        }

        private enum PieceType
        {
            I,
            J,
            L,
            O,
            S,
            T,
            Z,
            _Count,
        }
        private enum PieceRotation
        {
            One,
            Two,
            Three,
            Four,
            _Count,
        }
    }
}
