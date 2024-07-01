using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class Region : IRegion
    {
        private readonly RegionSection[,] _sections;
        private readonly SpaceInfo _spaceInfo;

        internal Region(RegionInfo info, IReadOnlyList<RegionSectionInfo> sectionInfos, SpaceInfo spaceInfo, Area area)
        {
            if (area.Size.Width != spaceInfo.RegionTiles.Width
                || area.Size.Height == 0
                || area.Size.Height % spaceInfo.RegionTiles.Height != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(area));
            }

            Id = info.Id;
            SeasonId = info.SeasonId;
            _spaceInfo = spaceInfo;
            Area = area;

            var sectionCount = (area.Size / spaceInfo.SectionTiles).ToSize();

            if (sectionInfos.Count == 0 || sectionInfos.Count % sectionCount.Width != 0)
            {
                throw new InvalidOperationException();
            }

            var infoCount = new Point(sectionCount.Width, sectionInfos.Count / sectionCount.Width);

            _sections = new RegionSection[sectionCount.Height, sectionCount.Width];
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

            Sections = _sections.Cast<IRegionSection>().ToArray();
        }

        public RegionId Id { get; }

        public SeasonId? SeasonId { get; }

        public Area Area { get; }

        public IReadOnlyList<IRegionSection> Sections { get; }

        public Size Count => new(_sections.GetLength(1), _sections.GetLength(0));

        internal Vector2 GetWarpPosition(Vector2? fromPosition)
        {
            var sections = _sections;
            var index = new Point(
                fromPosition is null ? (sections.GetLength(1) / 2) : GetSectionIndex(fromPosition.Value).X,
                sections.GetLength(0) - 1);
            var sectionArea = sections[index.Y, index.X].Area;

            const float Padding = 10;
            const float Width = 30;
            const float Height = 20;

            float width = Math.Min(Width, Math.Max(sectionArea.Size.Width - (Padding * 2), 0));
            float height = Math.Min(Height, Math.Max(sectionArea.Size.Height - (Padding * 2), 0));

            var area = Area.Rectangle(
                new PointF(sectionArea.Location.X + ((sectionArea.Size.Width - width) * 0.5f), sectionArea.Location.Y + Padding),
                new SizeF(width, height));

            return area.GetRandomPosition();
        }

        internal IRegionSection? Get(Vector2 position)
        {
            if (position == Vector2.Zero || !Area.Contains(position))
            {
                return null;
            }

            var index = GetSectionIndex(position);
            return _sections[index.Y, index.X];
        }

        private Point GetSectionIndex(Vector2 position)
        {
            var sections = _sections;
            var localPosition = position - Area.Location.ToVector2();
            return new Point(
                Math.Clamp((int)(localPosition.X / _spaceInfo.SectionTiles), 0, sections.GetLength(1) - 1),
                Math.Clamp((int)((Area.Size.Height - localPosition.Y) / _spaceInfo.SectionTiles), 0, sections.GetLength(0) - 1));
        }
    }
}
