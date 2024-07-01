using Some1.Play.Info;
using R3;
using System;

namespace Some1.Play.Front
{
    public interface IPlayerSpecialtyFront
    {
        SpecialtyId Id { get; }
        SeasonId Season { get; }
        RegionId Region { get; }
        ReadOnlyReactiveProperty<int> Number { get; }
        ReadOnlyReactiveProperty<Leveling> Star { get; }
        ReadOnlyReactiveProperty<TimeSpan?> NumberTimeAgo { get; }
    }
}
