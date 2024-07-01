using Some1.Play.Core.Internal;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;
using Some1.Play.Info.TestHelpers;
using ParallelOptions = Some1.Play.Core.Paralleling.ParallelOptions;

namespace Some1.Play.Core.TestHelpers;

internal sealed class ObjectFactoryServices
{
    internal ObjectFactoryServices() : this(new())
    {

    }

    internal ObjectFactoryServices(TestPlayInfoOptions infoOptions)
    {
        Info = TestPlayInfoFactory.Create(infoOptions);
        var clock = new FakeClock();
        Time = new Time(clock);
        ParallelOptions = new ParallelOptions() { Count = 1 };
        Space = new Space(Info.Space, ParallelOptions, Time);
        Regions = new RegionGroup(
            Info.Regions,
            Info.RegionSections,
            Info.Space);

        ObjectFactory = new(
            Info.Characters,
            Info.CharacterAlives,
            Info.CharacterIdles,
            Info.CharacterCasts,
            Info.CharacterCastStats,
            Info.CharacterStats,
            Info.CharacterEnergies,
            Info.CharacterSkins,
            Info.CharacterSkinEmojis,
            Info.Buffs,
            Info.BuffStats,
            Info.Boosters,
            Info.Specialties,
            Info.Triggers,
            ParallelOptions,
            Regions,
            Space,
            Time);

        LeaderToken = new();
        LeaderToken.SetArea(Space.Area);
        ParallelToken = new(0);
    }

    internal ObjectFactory ObjectFactory { get; }

    internal PlayInfo Info { get; }

    internal Time Time { get; }

    internal Space Space { get; }

    internal RegionGroup Regions { get; }

    internal LeaderToken LeaderToken { get; }

    internal ParallelOptions ParallelOptions { get; }

    internal ParallelToken ParallelToken { get; }

    internal void Update(float deltaSeconds)
    {
        Time.Update(deltaSeconds);
        Space.UpdateNonLeaderObjects(LeaderToken, ParallelToken);
        Space.UpdateBlocks(LeaderToken);
    }
}
