using System;
using System.Drawing;

namespace Some1.Play.Info
{
    public sealed class SpaceInfo
    {
        public SpaceInfo(
            Size spaceTiles,
            int blockTiles,
            Size regionTiles,
            int sectionTiles,
            int floorTiles,
            int brickTiles,
            int roadTiles,
            int plazaTiles,
            int fenceTiles,
            int entranceTiles)
        {
            if (spaceTiles.Width < 1 || spaceTiles.Height < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(spaceTiles));
            }

            if (blockTiles < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(blockTiles));
            }

            if (regionTiles.Width < 1 || regionTiles.Height < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(regionTiles));
            }

            if (sectionTiles < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(sectionTiles));
            }

            if (floorTiles < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(floorTiles));
            }

            if (brickTiles < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(brickTiles));
            }

            if (roadTiles < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(roadTiles));
            }

            if (plazaTiles < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(plazaTiles));
            }

            if (fenceTiles < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(fenceTiles));
            }

            if (entranceTiles < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(entranceTiles));
            }

            if (spaceTiles.Width % blockTiles != 0 || spaceTiles.Height % blockTiles != 0)
            {
                throw new InvalidOperationException("ERR1");
            }

            if (spaceTiles.Width != regionTiles.Width || spaceTiles.Height % regionTiles.Height != 0)
            {
                throw new InvalidOperationException("ERR2");
            }

            if (regionTiles.Width % sectionTiles != 0 || regionTiles.Height % sectionTiles != 0)
            {
                throw new InvalidOperationException("ERR3");
            }

            if (sectionTiles % brickTiles != 0)
            {
                throw new InvalidOperationException("ERR4");
            }

            if (sectionTiles % floorTiles != 0)
            {
                throw new InvalidOperationException("ERR5");
            }

            if (roadTiles % floorTiles != 0)
            {
                throw new InvalidOperationException("ERR6");
            }

            if (plazaTiles % floorTiles != 0)
            {
                throw new InvalidOperationException("ERR7");
            }

            if (fenceTiles % brickTiles != 0)
            {
                throw new InvalidOperationException("ERR8");
            }

            if (entranceTiles % brickTiles != 0)
            {
                throw new InvalidOperationException("ERR9");
            }

            SpaceTiles = spaceTiles;
            BlockTiles = blockTiles;
            RegionTiles = regionTiles;
            SectionTiles = sectionTiles;
            FloorTiles = floorTiles;
            BrickTiles = brickTiles;
            RoadTiles = roadTiles;
            PlazaTiles = plazaTiles;
            FenceTiles = fenceTiles;
            EntranceTiles = entranceTiles;
            SpaceRegions = new(spaceTiles.Width / regionTiles.Width, spaceTiles.Height / regionTiles.Height);
            RegionSections = new(regionTiles.Width / sectionTiles, regionTiles.Height / sectionTiles);
            SectionBricks = sectionTiles / brickTiles;
            RoadFloors = roadTiles / floorTiles;
            PlazaFloors = plazaTiles / floorTiles;
            FenceBricks = fenceTiles / brickTiles;
            EntranceBricks = entranceTiles / brickTiles;
        }

        public Size SpaceTiles { get; }

        public int BlockTiles { get; }

        public Size RegionTiles { get; }

        public int SectionTiles { get; }

        public int FloorTiles { get; }

        public int BrickTiles { get; }

        public int RoadTiles { get; }

        public int PlazaTiles { get; }

        public int FenceTiles { get; }

        public int EntranceTiles { get; }

        public Size SpaceBlocks { get; }

        public Size SpaceRegions { get; }

        public Size RegionSections { get; }

        public int SectionBricks { get; }

        public int RoadFloors { get; }

        public int PlazaFloors { get; }

        public int FenceBricks { get; }

        public int EntranceBricks { get; }
    }
}
