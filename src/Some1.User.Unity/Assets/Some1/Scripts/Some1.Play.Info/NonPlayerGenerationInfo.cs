using System;

namespace Some1.Play.Info
{
    public sealed class NonPlayerGenerationInfo
    {
        public NonPlayerGenerationInfo(
            CharacterId fenceId,
            CharacterId landmarkId,
            NonPlayerSpawningCoreInfo barrier,
            NonPlayerSpawningCoreInfo water,
            RegionSectionId regionSectionId)
        {
            FenceId = fenceId;
            LandmarkId = landmarkId;
            Barrier = barrier ?? throw new ArgumentNullException(nameof(barrier));
            Water = water ?? throw new ArgumentNullException(nameof(water));
            RegionSectionId = regionSectionId;
        }

        public CharacterId FenceId { get; }
        public CharacterId LandmarkId { get; }
        public NonPlayerSpawningCoreInfo Barrier { get; }
        public NonPlayerSpawningCoreInfo Water { get; }
        public RegionSectionId RegionSectionId { get; }
    }
}
