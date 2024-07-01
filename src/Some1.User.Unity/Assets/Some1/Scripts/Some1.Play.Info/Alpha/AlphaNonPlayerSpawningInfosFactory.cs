using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Some1.Play.Info.Alpha
{
    public static class AlphaNonPlayerSpawningInfosFactory
    {
        private const int FenceSize = 8;

        private const float Period5 = Period3 * 2;
        private const float Period4 = Period3 * 1.5f;
        private const float Period3 = 20;
        private const float Period2 = Period3 / 1.5f;
        private const float Period1 = Period3 / 2;

        private const float Density5 = Density3 * 2;
        private const float Density4 = Density3 * 1.5f;
        private const float Density3 = 1f / 20;
        private const float Density2 = Density3 / 1.5f;
        private const float Density1 = Density3 / 2;

        public static IEnumerable<NonPlayerSpawningInfo> Create()
        {
            var infos = new List<NonPlayerSpawningInfo>();

            //infos.AddRange(CreateTestInfos2(CharacterId.Chief2));

            infos.AddRange(CreateRegion1());
            infos.AddRange(CreateRegionAlpha());

            return infos;
        }

        private static IEnumerable<NonPlayerSpawningInfo> CreateRegionAlpha()
            => CreateYardInfos(
                CharacterId.Animal1,
                CharacterId.Animal2,
                CharacterId.Animal3);

        private static IEnumerable<NonPlayerSpawningInfo> CreateRegion1()
            => CreateFieldInfos(
                RegionId.Region1,
                CharacterId.Mob1,
                CharacterId.Mob2,
                CharacterId.Mob3,
                CharacterId.Mob4,
                CharacterId.Chief1,
                CharacterId.Chief2,
                CharacterId.Boss1,
                CharacterId.Boss2,
                CharacterId.Boss3,
                CharacterId.Boss4,
                CharacterId.Boss5,
                CharacterId.Boss6,
                CharacterId.Plant1,
                CharacterId.Plant2,
                CharacterId.Plant3,
                CharacterId.Plant5,
                CharacterId.Plant6,
                CharacterId.Plant7,
                CharacterId.Animal4,
                CharacterId.Animal5)
            .Concat(CreateRoadworks(RegionId.Region1));

        //private static IEnumerable<NonPlayerSpawningInfo> CreateTestInfos(params CharacterId[] characterIds)
        //{
        //    return Enumerable.Range(6, 3)
        //        .SelectMany(x => characterIds.Select(y => Create(y, x)));

        //    static NonPlayerSpawningInfo Create(CharacterId characterId, int sectionId)
        //    {
        //        return new NonPlayerSpawningInfo(
        //            new(
        //                characterId,
        //                new(0, 10),
        //                new(Vector2.Zero, Vector2.One, Vector2.Zero, new(FenceSize, FenceSize), new(FenceSize, FenceSize)),
        //                new(Density4, true, true)),
        //            new(RegionId.Region1, sectionId));
        //    }
        //}

        //private static IEnumerable<NonPlayerSpawningInfo> CreateTestInfos2(params CharacterId[] characterIds)
        //{
        //    foreach (var item in characterIds)
        //    {
        //        yield return new NonPlayerSpawningInfo(
        //            new(
        //                item,
        //                new(0, 10),
        //                new(Vector2.Zero, Vector2.One, Vector2.Zero, Vector2.Zero, Vector2.Zero),
        //                new(0.267f, true, false)),
        //            new(RegionId.Region1, 7));
        //    }
        //}

        private static IEnumerable<NonPlayerSpawningInfo> CreateYardInfos(
            CharacterId animal1,
            CharacterId animal2,
            CharacterId animal3)
        {
            return Enumerable.Range(0, 6)
                .Select(x => new RegionSectionId(RegionId.RegionAlpha, x))
                .SelectMany(x => CreateYardInfos(x, animal1, animal2, animal3));

            static IEnumerable<NonPlayerSpawningInfo> CreateYardInfos(
                RegionSectionId regionSectionId,
                CharacterId animal1,
                CharacterId animal2,
                CharacterId animal3)
            {
                return new CharacterId[] { animal1, animal2, animal3 }
                    .Select(x => new NonPlayerSpawningInfo(
                        new(
                            x,
                            new(0, Period3),
                            new(Vector2.Zero, Vector2.One, Vector2.Zero, new(FenceSize, FenceSize), new(FenceSize, FenceSize)),
                            new(Density3, true, true)),
                        regionSectionId,
                        Team.Player));
            }
        }

        private static IEnumerable<NonPlayerSpawningInfo> CreateFieldInfos(
            RegionId regionId,
            CharacterId mob1,
            CharacterId mob2,
            CharacterId mob3,
            CharacterId mob4,
            CharacterId chief1,
            CharacterId chief2,
            CharacterId boss1,
            CharacterId boss2,
            CharacterId boss3,
            CharacterId boss4,
            CharacterId boss5,
            CharacterId boss6,
            CharacterId plant1,
            CharacterId plant2,
            CharacterId plant3,
            CharacterId plant4,
            CharacterId plant5,
            CharacterId plant6,
            CharacterId animal1,
            CharacterId animal2)
        {
            return new[]
            {
                CreateLowFieldInfos(new(regionId, 6), mob1, mob2, mob3, mob4, chief1, chief2, animal1, animal2),
                CreateLowFieldInfos(new(regionId, 7), mob1, mob2, mob3, mob4, chief1, chief2, animal1, animal2),
                CreateLowFieldInfos(new(regionId, 8), mob1, mob2, mob3, mob4, chief1, chief2, animal1, animal2),
                CreateHighFieldInfos(new(regionId, 3), mob1, mob2, mob3, mob4, boss1, plant1),
                CreateHighFieldInfos(new(regionId, 4), mob1, mob2, mob3, mob4, boss2, plant2),
                CreateHighFieldInfos(new(regionId, 5), mob1, mob2, mob3, mob4, boss3, plant3),
                CreateHighFieldInfos(new(regionId, 0), mob1, mob2, mob3, mob4, boss4, plant4),
                CreateHighFieldInfos(new(regionId, 1), mob1, mob2, mob3, mob4, boss5, plant5),
                CreateHighFieldInfos(new(regionId, 2), mob1, mob2, mob3, mob4, boss6, plant6),
            }.SelectMany(x => x);

            static IEnumerable<NonPlayerSpawningInfo> CreateLowFieldInfos(
                RegionSectionId regionSectionId,
                CharacterId mob1,
                CharacterId mob2,
                CharacterId mob3,
                CharacterId mob4,
                CharacterId chief1,
                CharacterId chief2,
                CharacterId animal1,
                CharacterId animal2)
            {
                yield return CreateBoxInfo(regionSectionId);
                yield return CreateInfo(regionSectionId, mob1, Period1, Density3);
                yield return CreateInfo(regionSectionId, mob2, Period1, Density3);
                yield return CreateInfo(regionSectionId, mob3, Period1, Density3);
                yield return CreateInfo(regionSectionId, mob4, Period1, Density3);
                yield return CreateInfo(regionSectionId, chief1, Period3, Density1);
                yield return CreateInfo(regionSectionId, chief2, Period3, Density1);
                yield return CreateInfo(regionSectionId, animal1, Period3, Density2, Team.Player);
                yield return CreateInfo(regionSectionId, animal2, Period3, Density2, Team.Player);

                static NonPlayerSpawningInfo CreateInfo(RegionSectionId regionSectionId, CharacterId characterId, float period, float density, byte team = Team.Environment)
                {
                    return new NonPlayerSpawningInfo(
                        new(
                            characterId,
                            new(0, period),
                            new(Vector2.Zero, Vector2.One, Vector2.Zero, new(FenceSize, FenceSize), new(FenceSize, FenceSize)),
                            new(density, true, true)),
                        regionSectionId,
                        team);
                }
            }

            static IEnumerable<NonPlayerSpawningInfo> CreateHighFieldInfos(
                RegionSectionId regionSectionId,
                CharacterId mob1,
                CharacterId mob2,
                CharacterId mob3,
                CharacterId mob4,
                CharacterId boss,
                CharacterId plant)
            {
                yield return CreateBoxInfo(regionSectionId);
                yield return CreateMobInfo(regionSectionId, mob1);
                yield return CreateMobInfo(regionSectionId, mob2);
                yield return CreateMobInfo(regionSectionId, mob3);
                yield return CreateMobInfo(regionSectionId, mob4);
                yield return CreateBossInfo(regionSectionId, boss);
                yield return CreatePlantInfo(regionSectionId, plant);

                static NonPlayerSpawningInfo CreateMobInfo(RegionSectionId regionSectionId, CharacterId characterId)
                {
                    return new NonPlayerSpawningInfo(
                        new(
                            characterId,
                            new(0, Period3),
                            new(Vector2.Zero, Vector2.One, Vector2.Zero, new(FenceSize, FenceSize), new(FenceSize, FenceSize)),
                            new(Density1, true, true)),
                        regionSectionId);
                }

                static NonPlayerSpawningInfo CreateBossInfo(RegionSectionId regionSectionId, CharacterId characterId)
                {
                    return new NonPlayerSpawningInfo(
                        new(
                            characterId,
                            new(0, Period5),
                            new(new(1 / 6f), new(5 / 6f), Vector2.Zero, Vector2.Zero, Vector2.Zero),
                            new(Density1, true, true)),
                        regionSectionId);
                }

                static NonPlayerSpawningInfo CreatePlantInfo(RegionSectionId regionSectionId, CharacterId characterId)
                {
                    return new NonPlayerSpawningInfo(
                        new(
                            characterId,
                            new(0, Period3),
                            new(new(2 / 6f), new(4 / 6f), Vector2.Zero, Vector2.Zero, Vector2.Zero),
                            new(Density2, true, true)),
                        regionSectionId);
                }
            }
        }

        private static NonPlayerSpawningInfo CreateBoxInfo(RegionSectionId regionSectionId, float period = Period3, float density = Density3)
        {
            return new NonPlayerSpawningInfo(
                new(
                    CharacterId.Box,
                    new(0, period),
                    new(Vector2.Zero, Vector2.One, Vector2.Zero, new(FenceSize, FenceSize), new(FenceSize, FenceSize)),
                    new(density, true, true)),
                regionSectionId);
        }

        private static IEnumerable<NonPlayerSpawningInfo> CreateRoadworks(RegionId regionId)
        {
            return Enumerable.Range(0, 3)
                .Select(x => new NonPlayerSpawningInfo(
                    new(
                        CharacterId.Roadworks,
                        new(),
                        new(new(0.5f, 1), new(0.5f, 1), Vector2.Zero, new(0, -(FenceSize + 1)), new(0.01f, 0.01f)),
                        new(1, true, true)),
                    new(regionId, x)));
        }
    }
}
