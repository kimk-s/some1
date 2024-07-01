using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using Some1.Play.Info;

namespace Some1.Play.Front
{
    internal sealed class RegionGroupFront : IRegionGroupFront
    {
        private readonly SpaceInfo _spaceInfo;
        private readonly Area _area;
        private readonly RegionFront[] _all;

        internal RegionGroupFront(RegionInfoGroup infos, RegionSectionInfoGroup sectionInfos, SpaceInfo spaceInfo)
        {
            _spaceInfo = spaceInfo;
            _area = Area.Rectangle(PointF.Empty, spaceInfo.SpaceTiles);

            int n = 0;
            _all = new RegionFront[infos.ById.Count];
            foreach (var x in infos.ById.Values.OrderByDescending(x => x.Id))
            {
                var area = Area.Rectangle(
                    new PointF(0, spaceInfo.RegionTiles.Height * n),
                    new SizeF(
                        spaceInfo.RegionTiles.Width,
                        n == _all.Length - 1
                            ? spaceInfo.SpaceTiles.Height - spaceInfo.RegionTiles.Height * n
                            : spaceInfo.RegionTiles.Height));

                _all[n++] = new RegionFront(x, sectionInfos.ById[x.Id], spaceInfo, area);
            }

            All = _all.ToDictionary(x => x.Id, x => (IRegionFront)x);

            BySeasonId = _all
                .Where(x => x.SeasonId is not null)
                .ToDictionary(x => x.SeasonId!.Value, x => (IRegionFront)x);
        }

        public IReadOnlyDictionary<RegionId, IRegionFront> All { get; }

        public IReadOnlyDictionary<SeasonId, IRegionFront> BySeasonId { get; }

        public IRegionSectionFront? Get(Vector2 position)
        {
            if (position == Vector2.Zero || !_area.Contains(position))
            {
                return null;
            }

            var all = _all;
            if (position.Y < all[^1].Area.Location.Y)
            {
                var localPosition = position - _area.Location.ToVector2();
                int index = Math.Clamp(
                    (int)(localPosition.Y / _spaceInfo.RegionTiles.Height),
                    0,
                    all.Length - 1);
                return all[index].Get(position);
            }
            else
            {
                return all[^1].Get(position);
            }
        }
    }
}
