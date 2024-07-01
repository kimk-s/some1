using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class RegionGroup : IRegionGroup
    {
        private readonly SpaceInfo _spaceInfo;
        private readonly Area _area;
        private readonly Region[] _all;

        internal RegionGroup(RegionInfoGroup infos, RegionSectionInfoGroup sectionInfos, SpaceInfo spaceInfo)
        {
            _spaceInfo = spaceInfo;
            _area = Area.Rectangle(PointF.Empty, spaceInfo.SpaceTiles);

            int n = 0;
            _all = new Region[infos.ById.Count];
            foreach (var x in infos.ById.Values.OrderByDescending(x => x.Id))
            {
                var area = Area.Rectangle(
                    new PointF(0, spaceInfo.RegionTiles.Height * n),
                    new SizeF(
                        spaceInfo.RegionTiles.Width,
                        n == _all.Length - 1
                            ? spaceInfo.SpaceTiles.Height - spaceInfo.RegionTiles.Height * n
                            : spaceInfo.RegionTiles.Height));

                _all[n++] = new Region(x, sectionInfos.ById[x.Id], spaceInfo, area);
            }

            All = _all.ToDictionary(x => x.Id, x => (IRegion)x);
        }

        public IReadOnlyDictionary<RegionId, IRegion> All { get; }

        public IRegionSection Get(RegionSectionId id) => All[id.Region].Sections[id.Section];

        public IRegionSection? Get(Vector2 position)
        {
            if (position == Vector2.Zero || !_area.Contains(position))
            {
                return null;
            }

            if (position.Y < _all[^1].Area.Location.Y)
            {
                var localPosition = position - _area.Location.ToVector2();
                int index = Math.Clamp(
                    (int)(localPosition.Y / _spaceInfo.RegionTiles.Height),
                    0,
                    _all.Length - 1);
                return _all[index].Get(position);
            }
            else
            {
                return _all[^1].Get(position);
            }
        }

        public Vector2 GetTownWarpPosition(Vector2? fromPosition) => _all[0].GetWarpPosition(fromPosition);

        public Vector2 GetFieldWarpPosition(Vector2? fromPosition) => _all[1].GetWarpPosition(fromPosition);

        internal void Refresh(Vector2 position, ref IRegionSection? section)
        {
            if (section?.Area.Contains(position) ?? false)
            {
                return;
            }
            section = Get(position);
        }
    }
}
