using System;

namespace Some1.Play.Info
{
    public sealed class NonPlayerSpawningInfo
    {
        public NonPlayerSpawningInfo(NonPlayerSpawningCoreInfo core, RegionSectionId regionSectionId, byte team = Info.Team.Environment)
        {
            Core = core ?? throw new ArgumentNullException(nameof(core));
            RegionSectionId = regionSectionId;
            Team = team;
        }

        public NonPlayerSpawningCoreInfo Core { get; }
        public RegionSectionId RegionSectionId { get; }
        public byte Team { get; }
    }
}
