using System;
using System.Collections.Generic;
using System.Drawing;

namespace Some1.Play.Core.Internal
{
    internal sealed class BlockTiles
    {
        private readonly Tile[,] _tiles;
        private readonly Rectangle _area;
        private readonly List<Object> _objects = new();

        internal BlockTiles(Tile[,] tiles, Rectangle area)
        {
            if (area.Left < 0
                || area.Right > tiles.GetLength(1)
                || area.Top < 0
                || area.Bottom > tiles.GetLength(0))
            {
                throw new InvalidOperationException();
            }

            _tiles = tiles;
            _area = area;
        }

        internal void Add(Object obj)
        {
            if (obj.Character.Info?.BumpLevel is not null)
            {
                _objects.Add(obj);
                Update();
            }
        }

        internal void Remove(Object obj)
        {
            if (_objects.Remove(obj))
            {
                Update();
            }
        }

        private void Update()
        {
            var tiles = _tiles;

            for (int y = _area.Top; y < _area.Bottom; y++)
            {
                for (int x = _area.Left; x < _area.Right; x++)
                {
                    tiles[y, x] = new(null);
                }
            }

            foreach (var item in _objects)
            {
                var area = GetArea((RectangleF)item.Properties.Area);

                for (int y = area.Top; y < area.Bottom; y++)
                {
                    for (int x = area.Left; x < area.Right; x++)
                    {
                        if (item.Character.Info is null)
                        {
                            throw new InvalidOperationException();
                        }

                        var itemBumpLevel = item.Character.Info.BumpLevel;
                        if (itemBumpLevel is null)
                        {
                            continue;
                        }

                        ref var tile = ref tiles[y, x];

                        if (itemBumpLevel.Value <= tile.BumpLevel)
                        {
                            continue;
                        }

                        tile = new(itemBumpLevel);
                    }
                }
            }
        }

        private Rectangle GetArea(RectangleF areaF)
        {
            var result = new Rectangle(
                (int)areaF.Location.X,
                (int)areaF.Location.Y,
                (int)areaF.Size.Width,
                (int)areaF.Size.Height);
            result.Intersect(_area);
            return result;
        }
    }
}
