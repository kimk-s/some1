using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using Some1.Play.Info;

namespace Some1.Play.Front
{
    internal sealed class RegionFront : IRegionFront
    {
        private readonly RegionSectionFront[,] _sections;
        private readonly SpaceInfo _spaceInfo;

        internal RegionFront(RegionInfo info, IReadOnlyList<RegionSectionInfo> sectionInfos, SpaceInfo spaceInfo, Area area)
        {
            if (area.Size.Width != spaceInfo.RegionTiles.Width
                || area.Size.Height == 0
                || area.Size.Height % spaceInfo.RegionTiles.Height != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(area));
            }

            Id = info.Id;
            _spaceInfo = spaceInfo;
            Area = area;
            SeasonId = info.SeasonId;

            var sectionCount = (area.Size / spaceInfo.SectionTiles).ToSize();

            if (sectionInfos.Count == 0 || sectionInfos.Count % sectionCount.Width != 0)
            {
                throw new InvalidOperationException();
            }

            var infoCount = new Point(sectionCount.Width, sectionInfos.Count / sectionCount.Width);

            _sections = new RegionSectionFront[sectionCount.Height, sectionCount.Width];
            for (int y = 0; y < _sections.GetLength(0); y++)
            {
                for (int x = 0; x < _sections.GetLength(1); x++)
                {
                    var sectionArea = Area.Rectangle(
                        new PointF(
                            area.Left + spaceInfo.SectionTiles * x,
                            area.Bottom - spaceInfo.SectionTiles * (y + 1)),
                        spaceInfo.SectionTiles);

                    int infoIndex = x + Math.Min(y, infoCount.Y - 1) * infoCount.X;

                    _sections[y, x] = new(sectionInfos[infoIndex], sectionArea);
                }
            }

            Sections = _sections.Cast<IRegionSectionFront>().ToArray();
        }

        public RegionId Id { get; }

        public Area Area { get; }

        public SeasonId? SeasonId { get; }

        public IReadOnlyList<IRegionSectionFront> Sections { get; }

        internal IRegionSectionFront? Get(Vector2 position)
        {
            if (position == Vector2.Zero || !Area.Contains(position))
            {
                return null;
            }

            var sections = _sections;
            var localPosition = position - Area.Location.ToVector2();
            var index = new Point(
                Math.Clamp((int)(localPosition.X / _spaceInfo.SectionTiles), 0, sections.GetLength(1) - 1),
                Math.Clamp((int)((Area.Size.Height - localPosition.Y) / _spaceInfo.SectionTiles), 0, sections.GetLength(0) - 1));
            return sections[index.Y, index.X];
        }
    }
}
