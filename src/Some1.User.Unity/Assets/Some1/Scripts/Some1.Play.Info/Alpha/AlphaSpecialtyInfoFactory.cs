using System.Collections.Generic;

namespace Some1.Play.Info.Alpha
{
    public static class AlphaSpecialtyInfoFactory
    {
        public static IEnumerable<SpecialtyInfo> Create() => new SpecialtyInfo[]
        {
            new(SpecialtyId.Mob1, SeasonId.Season1, RegionId.Region1),
            new(SpecialtyId.Mob2, SeasonId.Season1, RegionId.Region1),
            new(SpecialtyId.Mob3, SeasonId.Season1, RegionId.Region1),
            new(SpecialtyId.Mob4, SeasonId.Season1, RegionId.Region1),
            new(SpecialtyId.Chief1, SeasonId.Season1, RegionId.Region1),
            new(SpecialtyId.Chief2, SeasonId.Season1, RegionId.Region1),
            new(SpecialtyId.Boss1, SeasonId.Season1, RegionId.Region1),
            new(SpecialtyId.Boss2, SeasonId.Season1, RegionId.Region1),
            new(SpecialtyId.Boss3, SeasonId.Season1, RegionId.Region1),
            new(SpecialtyId.Boss4, SeasonId.Season1, RegionId.Region1),
            new(SpecialtyId.Boss5, SeasonId.Season1, RegionId.Region1),
            new(SpecialtyId.Boss6, SeasonId.Season1, RegionId.Region1),
            new(SpecialtyId.Plant1, SeasonId.Season1, RegionId.Region1),
            new(SpecialtyId.Plant2, SeasonId.Season1, RegionId.Region1),
            new(SpecialtyId.Plant3, SeasonId.Season1, RegionId.Region1),
            new(SpecialtyId.Plant4, SeasonId.Season1, RegionId.Region1),
            new(SpecialtyId.Plant5, SeasonId.Season1, RegionId.Region1),
            new(SpecialtyId.Plant6, SeasonId.Season1, RegionId.Region1),
            new(SpecialtyId.Plant7, SeasonId.Season1, RegionId.Region1),
            new(SpecialtyId.Plant8, SeasonId.Season1, RegionId.Region1),
            new(SpecialtyId.Animal1, SeasonId.Season1, RegionId.RegionAlpha),
            new(SpecialtyId.Animal2, SeasonId.Season1, RegionId.RegionAlpha),
            new(SpecialtyId.Animal3, SeasonId.Season1, RegionId.RegionAlpha),
            new(SpecialtyId.Animal4, SeasonId.Season1, RegionId.RegionAlpha),
            new(SpecialtyId.Animal5, SeasonId.Season1, RegionId.RegionAlpha),
            new(SpecialtyId.Animal6, SeasonId.Season1, RegionId.RegionAlpha),
            new(SpecialtyId.Animal7, SeasonId.Season1, RegionId.RegionAlpha),
            new(SpecialtyId.Animal8, SeasonId.Season1, RegionId.RegionAlpha),
            new(SpecialtyId.Animal9, SeasonId.Season1, RegionId.RegionAlpha),
            new(SpecialtyId.Animal10, SeasonId.Season1, RegionId.RegionAlpha),
        };
    }
}
