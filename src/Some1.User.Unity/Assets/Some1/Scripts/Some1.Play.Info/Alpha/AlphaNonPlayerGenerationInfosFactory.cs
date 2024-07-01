using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Some1.Play.Info.Alpha
{
    public static class AlphaNonPlayerGenerationInfosFactory
    {
        private const int FenceSize = 8;

        private const float Density5 = Density3 * 2;
        private const float Density4 = Density3 * 1.5f;
        private const float Density3 = 1f / 20;
        private const float Density2 = Density3 / 1.5f;
        private const float Density1 = Density3 / 2;

        public static IEnumerable<NonPlayerGenerationInfo> Create(AlphaPlayInfoEnvironment environment)
        {
            var infos = new List<NonPlayerGenerationInfo>();

            if (environment.IsProduction())
            {
                infos.AddRange(Create(RegionId.Region1, Density5));
                infos.AddRange(Create(RegionId.RegionAlpha, Density5));
            }
            else
            {
                //infos.AddRange(Enumerable.Range(6, 3).Select(x => Create(new RegionSectionId(RegionId.Region1, x), Density3)));
            }

            return infos;
        }

        private static IEnumerable<NonPlayerGenerationInfo> Create(RegionId regionId, float density)
            => Create(regionId, density, density);

        private static IEnumerable<NonPlayerGenerationInfo> Create(RegionId regionId, float barrierDensity, float waterDensity)
            => Enumerable.Range(0, 9).Select(x => Create(new RegionSectionId(regionId, x), barrierDensity, waterDensity));

        private static NonPlayerGenerationInfo Create(RegionSectionId regionSectionId, float density)
            => Create(regionSectionId, density, density);

        private static NonPlayerGenerationInfo Create(RegionSectionId regionSectionId, float barrierDensity, float waterDensity) => new(
            CharacterId.Fence1,
            CharacterId.Landmark1,
            new(
                CharacterId.Barrier1,
                default,
                new(Vector2.Zero, Vector2.One, default, new(FenceSize, FenceSize), new(FenceSize, FenceSize)),
                new(barrierDensity, true, true)),
            new(
                CharacterId.Water1,
                default,
                new(Vector2.Zero, Vector2.One, default, new(FenceSize, FenceSize), new(FenceSize, FenceSize)),
                new(waterDensity, true, true)),
            regionSectionId);
    }
}
