//using System.Drawing;
//using Some1.Play.Core.Options;
//using Some1.Play.Core.Paralleling;
//using Some1.Play.Core.TestHelpers;
//using Some1.Play.Info;
//using ParallelOptions = Some1.Play.Core.Paralleling.ParallelOptions;

//namespace Some1.Play.Core.Internal;

//public class LeaderTests
//{
//    [Fact]
//    public void Ctor()
//    {
//        var spaceInfo = new SpaceInfo(new(2, 2));
//        var spaceOptions = new SpaceOptions() { Block = new() { Size = new(1, 1) } };
//        var parallelOptions = new ParallelOptions() { Count = 1 };
//        var clock = new FakeClock(default, default);
//        var time = new Time(clock);
//        var space = new Space(spaceInfo, spaceOptions, parallelOptions, time);

//        var leader = new Leader(space, () => default);

//        Assert.False(leader.IsEnabled);
//        Assert.Equal(LeaderStatus.None, leader.Status);
//        Assert.Equal(default, leader.Token.Area);
//        Assert.Empty(leader.Token.ExtraBlockIds);
//    }

//    [Fact]
//    public void Enable()
//    {
//        var s = new Services(false);

//        s.Leader.Enable();

//        Assert.True(s.Leader.IsEnabled);
//    }

//    [Fact]
//    public void Enable_Already_Throws()
//    {
//        var s = new Services(false);
//        s.Leader.Enable();

//        Assert.Throws<InvalidOperationException>(s.Leader.Enable);
//    }

//    [Fact]
//    public void Disable()
//    {
//        var s = new Services(false);
//        s.Leader.Enable();

//        s.Leader.Disable();

//        Assert.True(!s.Leader.IsEnabled);
//    }

//    [Fact]
//    public void Disable_NotYet_Throws()
//    {
//        var s = new Services(false);

//        Assert.Throws<InvalidOperationException>(s.Leader.Disable);
//    }

//    public static IEnumerable<object[]> AreaData => new object[][]
//    {
//        new object[] { new RectangleF(0, 0, 1, 1) },
//        new object[] { new RectangleF(1, 0, 1, 1) },
//        new object[] { new RectangleF(0, 1, 1, 1) },
//        new object[] { new RectangleF(1, 1, 1, 1) },
//        new object[] { new RectangleF(0, 0, 1, 2) },
//        new object[] { new RectangleF(1, 0, 1, 2) },
//        new object[] { new RectangleF(0, 0, 2, 1) },
//        new object[] { new RectangleF(0, 1, 2, 1) },
//        new object[] { new RectangleF(0, 0, 2, 2) },
//        new object[] { new RectangleF(-1, -1, 4, 4) },
//        new object[] { new RectangleF(0, 0.5f, 1, 1) },
//        new object[] { new RectangleF(1, 0.5f, 1, 1) },
//        new object[] { new RectangleF(0.5f, 0, 1, 1) },
//        new object[] { new RectangleF(0.5f, 1, 1, 1) },
//        new object[] { new RectangleF(0.5f, 0.5f, 1, 1) },
//    };

//    [Theory]
//    [MemberData(nameof(AreaData))]
//    public void Lead1(RectangleF area)
//    {
//        var s = new Services(() => area, true);
//        s.Time.Update(1f);
//        s.Leader.Enable();
//        (LeaderStatus, ParallelToken)? eventArgs = null;
//        s.Leader.LeadCompleted += (_, e) => eventArgs = e;

//        var result = s.Leader.Lead1(s.ParallelToken);

//        Assert.True(result);
//        Assert.True(s.Leader.IsEnabled);
//        Assert.Equal(LeaderStatus.Lead1Completed, s.Leader.Status);
//        Assert.Equal(area, s.Leader.Token.Area);
//        Assert.All(s.Things.Where(x => x.Area.IntersectsWith(area)), x => Assert.True(x.Control.IsTaken));
//        Assert.All(s.Things.Where(x => !x.Area.IntersectsWith(area)), x => Assert.False(x.Control.IsTaken));
//        Assert.Equal((LeaderStatus.Lead1Completed, s.ParallelToken), eventArgs);
//    }

//    [Fact]
//    public void Lead1_BeforeEnable_False()
//    {
//        var s = new Services(false);
//        s.Time.Update(1f);

//        var result = s.Leader.Lead1(s.ParallelToken);

//        Assert.False(result);
//    }

//    [Theory]
//    [MemberData(nameof(AreaData))]
//    public void Lead2(RectangleF area)
//    {
//        var s = new Services(() => area, true);
//        s.Time.Update(1f);
//        s.Leader.Enable();
//        _ = s.Leader.Lead1(s.ParallelToken);
//        var extraBlockIds = s.Leader.Token.ExtraBlockIds.ToArray();
//        (LeaderStatus, ParallelToken)? eventArgs = null;
//        s.Leader.LeadCompleted += (_, e) => eventArgs = e;

//        var result = s.Leader.Lead2(s.ParallelToken);

//        Assert.True(result);
//        Assert.True(s.Leader.IsEnabled);
//        Assert.Equal(LeaderStatus.Lead2Completed, s.Leader.Status);
//        Assert.Equal(default, s.Leader.Token.Area);
//        Assert.All(s.Space.Blocks.Where(x => x.Area.IntersectsWith(area) || extraBlockIds.Contains(x.Id)), x => Assert.True(x.Control.IsTaken));
//        Assert.All(s.Space.Blocks.Where(x => !x.Area.IntersectsWith(area) && !extraBlockIds.Contains(x.Id)), x => Assert.False(x.Control.IsTaken));
//        Assert.Equal((LeaderStatus.Lead2Completed, s.ParallelToken), eventArgs);
//    }

//    [Fact]
//    public void Lead2_BeforeLead1_False()
//    {
//        var s = new Services(false);
//        s.Time.Update(1f);
//        s.Leader.Enable();

//        var result = s.Leader.Lead2(s.ParallelToken);

//        Assert.False(result);
//    }

//    [Fact]
//    public void Lead3()
//    {
//        var s = new Services(false);
//        s.Time.Update(1f);
//        s.Leader.Enable();
//        _ = s.Leader.Lead1(s.ParallelToken);
//        _ = s.Leader.Lead2(s.ParallelToken);
//        (LeaderStatus, ParallelToken)? eventArgs = null;
//        s.Leader.LeadCompleted += (_, e) => eventArgs = e;

//        var result = s.Leader.Lead3(s.ParallelToken);

//        Assert.True(result);
//        Assert.True(s.Leader.IsEnabled);
//        Assert.Equal(LeaderStatus.Lead3Completed, s.Leader.Status);
//        Assert.Equal(default, s.Leader.Token.Area);
//        Assert.Equal((LeaderStatus.Lead3Completed, s.ParallelToken), eventArgs);
//    }

//    [Fact]
//    public void Lead3_BeforeLead2_False()
//    {
//        var s = new Services(false);
//        s.Time.Update(1f);
//        s.Leader.Enable();
//        _ = s.Leader.Lead1(s.ParallelToken);

//        var result = s.Leader.Lead3(s.ParallelToken);

//        Assert.False(result);
//    }

//    private sealed class Services
//    {
//        public Services(bool withThings) : this(() => default, withThings)
//        {

//        }

//        public Services(Func<Area> areaGetter, bool withThings)
//        {
//            var spaceInfo = new SpaceInfo(new(2, 2));
//            var spaceOptions = new SpaceOptions() { Block = new() { Size = new(1, 1) } };
//            ParallelOptions = new ParallelOptions() { Count = 1 };
//            var clock = new FakeClock(default, default);
//            Time = new Time(clock);
//            Space = new Space(spaceInfo, spaceOptions, ParallelOptions, Time);

//            Leader = new Leader(Space, areaGetter);

//            Things = withThings
//                ? AreaData.Select(x =>
//                {
//                    var thing = CreateThing();
//                    Space.Set(thing, (RectangleF)x[0], null);
//                    return thing;
//                }).ToArray()
//                : Array.Empty<Thing>();

//            ParallelToken = new(0);
//        }

//        public Leader Leader { get; }

//        public Space Space { get; }

//        public ParallelOptions ParallelOptions { get; }

//        public Time Time { get; }

//        public IEnumerable<Thing> Things { get; }

//        public ParallelToken ParallelToken { get; }

//        public Thing CreateThing()
//        {
//            return new Thing(Time);
//        }
//    }
//}
