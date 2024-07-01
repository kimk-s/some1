//using System.Drawing;
//using System.Numerics;
//using Some1.Play.Core.Options;
//using Some1.Play.Core.Paralleling;
//using Some1.Play.Core.TestHelpers;
//using Some1.Play.Info;
//using ParallelOptions = Some1.Play.Core.Paralleling.ParallelOptions;

//namespace Some1.Play.Core.Internal;

//public class SpaceTests
//{
//    [Fact]
//    public void Ctor()
//    {
//        var info = new SpaceInfo(new(2, 2));
//        var options = new SpaceOptions { Block = new() { Size = new(1, 1) } };
//        var parallelOptions = new ParallelOptions { Count = 1 };
//        var clock = new FakeClock(default, default);
//        var time = new Time(clock);

//        var space = new Space(info, options, parallelOptions, time);

//        Size blockCount = new((int)MathF.Ceiling(info.Size.Width / options.Block.Size.Width), (int)MathF.Ceiling(info.Size.Height / options.Block.Size.Height));
//        var blockIds = Enumerable.Range(0, blockCount.Height).SelectMany(y => Enumerable.Range(0, blockCount.Width).Select(x => new BlockId(x, y)));
//        Assert.Equal(Area.Rectangle(PointF.Empty, info.Size), space.Area);
//        Assert.Equal(blockCount.Width * blockCount.Height, space.BlockCount);
//        Assert.Equal(blockIds, space.Blocks.Select(x => x.Id));
//    }

//    public static IEnumerable<object[]> GetBlockData => new object[][]
//    {
//        new object[] { new BlockId(0, 0) },
//        new object[] { new BlockId(1, 0) },
//        new object[] { new BlockId(0, 1) },
//        new object[] { new BlockId(1, 1) },
//    };

//    [Theory]
//    [MemberData(nameof(GetBlockData))]
//    public void GetBlock(BlockId id)
//    {
//        var s = new Services(false);

//        var block = s.Space.GetBlock(id);

//        Assert.Equal(id, block.Id);
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
//    public void GetBlocks(RectangleF area)
//    {
//        var s = new Services(false);

//        var blocks = s.Space.GetBlocks(area);

//        Assert.Equal(s.Space.Blocks.Where(x => x.Area.IntersectsWith(area)), blocks);
//    }

//    [Theory]
//    [MemberData(nameof(AreaData))]
//    public void GetThings(RectangleF area)
//    {
//        var s = new Services(true);

//        var things = s.Space.GetObjects(area);

//        Assert.Equal(s.Things.Where(x => x.Area.IntersectsWith(area)).OrderBy(x => x.Id), things.OrderBy(x => x.Id));
//    }

//    [Theory]
//    [MemberData(nameof(AreaData))]
//    public void SetThingTo(RectangleF area)
//    {
//        var s = new Services(false);
//        var thing = s.CreateThing();

//        s.Space.Set(thing, area, null);

//        Assert.Equal(area, thing.Area);
//        AssertThingContainsInCorrectBlocks(thing, s);
//    }

//    [Theory]
//    [InlineData(-1, 0)]
//    [InlineData(0, -1)]
//    [InlineData(-1, -1)]
//    [InlineData(1, 0)]
//    [InlineData(0, 1)]
//    [InlineData(1, 1)]
//    public void Move_True(float x, float y)
//    {
//        var s = new Services(false);
//        var thing = s.CreateThing();
//        s.Space.Set(thing, new RectangleF(1, 1, 1, 1), null);

//        var result = s.Space.Move(thing, new(x, y), null);

//        Assert.True(result);
//        Assert.Equal(new RectangleF(1 + x, 1 + y, 1, 1), thing.Area);
//        AssertThingContainsInCorrectBlocks(thing, s);
//    }

//    [Fact]
//    public void Move_ToOutOfLeftTopSpace_False()
//    {
//        var s = new Services(false);
//        var thing = s.CreateThing();
//        s.Space.Set(thing, new RectangleF(0.5f, 0.5f, 1, 1), null);

//        var result = s.Space.Move(thing, new(-1, -1), null);

//        Assert.False(result);
//        Assert.Equal(new RectangleF(0, 0, 1, 1), thing.Area);
//        AssertThingContainsInCorrectBlocks(thing, s);
//    }

//    [Fact]
//    public void Move_ToOutOfRightBottomSpace_False()
//    {
//        var s = new Services(false);
//        var thing = s.CreateThing();
//        s.Space.Set(thing, new RectangleF(18.5f, 18.5f, 1, 1), null);

//        var result = s.Space.Move(thing, new(1, 1), null);

//        Assert.False(result);
//        Assert.Equal(new RectangleF(19, 19, 1, 1), thing.Area);
//        AssertThingContainsInCorrectBlocks(thing, s);
//    }

//    [Fact]
//    public void Move_FromNoneToLeftTopWall_True()
//    {
//        var s = new Services(false);
//        var flyer = s.CreateThing();
//        flyer.Character = new(null, ColliderType.None, CharacterTargetInfo.False());
//        s.Space.Set(flyer, new RectangleF(0.5f, 0.5f, 1, 1), null);
//        var wall = s.CreateThing();
//        wall.Character = new(null, ColliderType.Wall, CharacterTargetInfo.False());
//        wall.IsAlive = true;
//        wall.Skin = new(null, true);
//        s.Space.Set(wall, new RectangleF(2, 2, 1, 1), null);

//        var result = s.Space.Move(flyer, new(1, 1), null);

//        Assert.True(result);
//        Assert.Equal(new RectangleF(1.5f, 1.5f, 1, 1), flyer.Area);
//        AssertThingContainsInCorrectBlocks(flyer, s);
//    }

//    [Fact]
//    public void Move_FromUnitToLeftTopWall_False()
//    {
//        var s = new Services(false);
//        var flyer = s.CreateThing();
//        flyer.Character = new(null, ColliderType.Unit, CharacterTargetInfo.False());
//        s.Space.Set(flyer, new RectangleF(0.5f, 0.5f, 1, 1), null);
//        var wall = s.CreateThing();
//        wall.Character = new(null, ColliderType.Wall, CharacterTargetInfo.False());
//        wall.IsAlive = true;
//        wall.Skin = new(null, true);
//        s.Space.Set(wall, new RectangleF(2, 2, 1, 1), null);

//        var result = s.Space.Move(flyer, new(1, 1), null);

//        Assert.False(result);
//        Assert.Equal(new RectangleF(1, 1, 1, 1), flyer.Area);
//        AssertThingContainsInCorrectBlocks(flyer, s);
//    }

//    [Fact]
//    public void Move_FromWalkerToRightBottomWall_False()
//    {
//        var s = new Services(false);
//        var flyer = s.CreateThing();
//        flyer.Character = new(null, ColliderType.Unit, CharacterTargetInfo.False());
//        s.Space.Set(flyer, new RectangleF(2.5f, 2.5f, 1, 1), null);
//        var wall = s.CreateThing();
//        wall.Character = new(null, ColliderType.Wall, CharacterTargetInfo.False());
//        wall.IsAlive = true;
//        wall.Skin = new(null, true);
//        s.Space.Set(wall, new RectangleF(1, 1, 1, 1), null);

//        var result = s.Space.Move(flyer, new(-1, -1), null);

//        Assert.False(result);
//        Assert.Equal(new RectangleF(2, 2, 1, 1), flyer.Area);
//        AssertThingContainsInCorrectBlocks(flyer, s);
//    }

//    [Fact]
//    public void Move_ZeroThingAreaSize_Throws()
//    {
//        var s = new Services(false);
//        var thing = s.CreateThing();

//        Assert.Throws<InvalidOperationException>(() => s.Space.Move(thing, Vector2.Zero, null));
//    }

//    [Fact]
//    public void Move_OverMaxDelta_Throws()
//    {
//        var s = new Services(false);
//        var thing = s.CreateThing();
//        s.Space.Set(thing, new RectangleF(0, 0, 1, 1), null);

//        Assert.Throws<ArgumentOutOfRangeException>(() => s.Space.Move(thing, new(PlayConstants.MaxMoveDeltaLength + 1, 0), null));
//    }

//    [Theory]
//    [InlineData(-1, 0)]
//    [InlineData(0, -1)]
//    [InlineData(-1, -1)]
//    [InlineData(1, 0)]
//    [InlineData(0, 1)]
//    [InlineData(1, 1)]
//    public void Teleport_True(float x, float y)
//    {
//        var s = new Services(false);
//        var thing = s.CreateThing();
//        s.Space.Set(thing, new RectangleF(1, 1, 1, 1), null);

//        var result = s.Space.Teleport(thing, new(x, y), null);

//        Assert.True(result);
//        Assert.Equal(new RectangleF(1 + x, 1 + y, 1, 1), thing.Area);
//        AssertThingContainsInCorrectBlocks(thing, s);
//    }

//    [Fact]
//    public void Teleport_ToOutOfLeftTopSpace_False()
//    {
//        var s = new Services(false);
//        var thing = s.CreateThing();
//        s.Space.Set(thing, new RectangleF(0.5f, 0.5f, 1, 1), null);

//        var result = s.Space.Teleport(thing, new(-1, -1), null);

//        Assert.False(result);
//        Assert.Equal(new RectangleF(0, 0, 1, 1), thing.Area);
//        AssertThingContainsInCorrectBlocks(thing, s);
//    }

//    [Fact]
//    public void Teleport_ToOutOfRightBottomSpace_False()
//    {
//        var s = new Services(false);
//        var thing = s.CreateThing();
//        s.Space.Set(thing, new RectangleF(18.5f, 18.5f, 1, 1), null);

//        var result = s.Space.Teleport(thing, new(1, 1), null);

//        Assert.False(result);
//        Assert.Equal(new RectangleF(19, 19, 1, 1), thing.Area);
//        AssertThingContainsInCorrectBlocks(thing, s);
//    }

//    [Fact]
//    public void Teleport_FromFlyerToLeftTopWall_True()
//    {
//        var s = new Services(false);
//        var flyer = s.CreateThing();
//        flyer.Character = new(null, ColliderType.None, CharacterTargetInfo.False());
//        s.Space.Set(flyer, new RectangleF(0.5f, 0.5f, 1, 1), null);
//        var wall = s.CreateThing();
//        wall.Character = new(null, ColliderType.Wall, CharacterTargetInfo.False());
//        wall.IsAlive = true;
//        wall.Skin = new(null, true);
//        s.Space.Set(wall, new RectangleF(2, 2, 1, 1), null);

//        var result = s.Space.Teleport(flyer, new(1, 1), null);

//        Assert.True(result);
//        Assert.Equal(new RectangleF(1.5f, 1.5f, 1, 1), flyer.Area);
//        AssertThingContainsInCorrectBlocks(flyer, s);
//    }

//    [Fact]
//    public void Teleport_FromWalkerToLeftTopWall_False()
//    {
//        var s = new Services(false);
//        var flyer = s.CreateThing();
//        flyer.Character = new(null, ColliderType.Unit, CharacterTargetInfo.False());
//        s.Space.Set(flyer, new RectangleF(0.5f, 0.5f, 1, 1), null);
//        var wall = s.CreateThing();
//        wall.Character = new(null, ColliderType.Wall, CharacterTargetInfo.False());
//        wall.IsAlive = true;
//        wall.Skin = new(null, true);
//        s.Space.Set(wall, new RectangleF(2, 2, 1, 1), null);

//        var result = s.Space.Teleport(flyer, new(1, 1), null);

//        Assert.False(result);
//        Assert.Equal(new RectangleF(3, 3, 1, 1), flyer.Area);
//        AssertThingContainsInCorrectBlocks(flyer, s);
//    }

//    [Fact]
//    public void Teleport_FromWalkerToRightBottomWall_False()
//    {
//        var s = new Services(false);
//        var flyer = s.CreateThing();
//        flyer.Character = new(null, ColliderType.Unit, CharacterTargetInfo.False());
//        s.Space.Set(flyer, new RectangleF(2.5f, 2.5f, 1, 1), null);
//        var wall = s.CreateThing();
//        wall.Character = new(null, ColliderType.Wall, CharacterTargetInfo.False());
//        wall.IsAlive = true;
//        wall.Skin = new(null, true);
//        s.Space.Set(wall, new RectangleF(1, 1, 1, 1), null);

//        var result = s.Space.Teleport(flyer, new(-1, -1), null);

//        Assert.False(result);
//        Assert.Equal(new RectangleF(0, 0, 1, 1), flyer.Area);
//        AssertThingContainsInCorrectBlocks(flyer, s);
//    }

//    [Fact]
//    public void Teleport_ZeroThingAreaSize_Throws()
//    {
//        var s = new Services(false);
//        var thing = s.CreateThing();

//        Assert.Throws<InvalidOperationException>(() => s.Space.Teleport(thing, Vector2.Zero, null));
//    }

//    [Fact]
//    public void Teleport_OverMaxDelta_Throws()
//    {
//        var s = new Services(false);
//        var thing = s.CreateThing();
//        s.Space.Set(thing, new RectangleF(0, 0, 1, 1), null);

//        Assert.Throws<ArgumentOutOfRangeException>(() => s.Space.Teleport(thing, new(PlayConstants.MaxTeleportDeltaLength + 1, 0), null));
//    }

//    [Theory]
//    [MemberData(nameof(AreaData))]
//    public void UpdateThings(RectangleF area)
//    {
//        var s = new Services(true);
//        s.LeaderToken.SetArea(area);
//        s.Time.Update(1f);

//        s.Space.UpdateObjects(s.LeaderToken, s.ParallelToken);

//        Assert.All(s.Things.Where(x => x.Area.IntersectsWith(area)), x => Assert.True(x.Control.IsTaken));
//        Assert.All(s.Things.Where(x => !x.Area.IntersectsWith(area)), x => Assert.False(x.Control.IsTaken));
//    }

//    [Fact]
//    public void UpdateThings_AddExtraBlockId()
//    {
//        var s = new Services(false);
//        var thing = s.CreateThing();
//        s.Space.Set(thing, Area.Rectangle(new PointF(0, 0), new(1, 1)), null);
//        s.LeaderToken.SetArea(thing.Area);
//        thing.UpdateCompleted += (_, e) =>
//        {
//            s.Space.Set(thing, new RectangleF(4, 0, 1, 1), e);
//        };
//        s.Time.Update(1f);

//        s.Space.UpdateObjects(s.LeaderToken, s.ParallelToken);

//        Assert.Equal(s.GetBlockIds(thing.Area).Location, s.LeaderToken.ExtraBlockIds.Single());
//    }

//    [Fact]
//    public void UpdateThings_NullArguments_Throws()
//    {
//        var s = new Services(false);

//        Assert.Throws<ArgumentNullException>(() => s.Space.UpdateObjects(s.LeaderToken, null!));
//        Assert.Throws<ArgumentNullException>(() => s.Space.UpdateObjects(null!, s.ParallelToken));
//    }

//    [Theory]
//    [MemberData(nameof(AreaData))]
//    public void UpdateBlocks(RectangleF area)
//    {
//        var s = new Services(false);
//        s.LeaderToken.SetArea(area);
//        s.Time.Update(1f);

//        s.Space.UpdateBlocks(s.LeaderToken);

//        Assert.All(s.Space.Blocks.Where(x => area.IntersectsWith(x.Area)), x => Assert.True(x.Control.IsTaken));
//        Assert.All(s.Space.Blocks.Where(x => !area.IntersectsWith(x.Area)), x => Assert.False(x.Control.IsTaken));
//    }

//    [Fact]
//    public void UpdateBlocks_WithExtraBlockIds()
//    {
//        var s = new Services(false);
//        s.LeaderToken.SetArea(Area.Rectangle(new PointF(0, 0), new(1, 1)));
//        var extraBlockId = new BlockId(s.GetBlockIds(Area.Rectangle(new PointF(1, 0), new(1, 1))).Location);
//        s.LeaderToken.AddExtraBlockId(extraBlockId);
//        s.Time.Update(1f);

//        s.Space.UpdateBlocks(s.LeaderToken);

//        Assert.True(s.Space.GetBlock(extraBlockId).Control.IsTaken);
//        Assert.Empty(s.LeaderToken.ExtraBlockIds);
//    }

//    private static void AssertThingContainsInCorrectBlocks(Thing thing, Services s)
//    {
//        Assert.All(s.Space.Blocks.Where(x => thing.Area.IntersectsWith(x.Area)), x => Assert.Single(x.Objects.Where(x => x == thing)));
//        Assert.All(s.Space.Blocks.Where(x => !thing.Area.IntersectsWith(x.Area)), x => Assert.Empty(x.Objects));
//    }

//    private sealed class Services
//    {
//        private readonly ObjectFactoryServices _ofs = new();

//        public Services(bool withThings)
//        {
//            var info = new SpaceInfo(new(20, 20));
//            Options = new SpaceOptions { Block = new() { Size = new(4, 4) } };
//            ParallelOptions = new ParallelOptions { Count = 1 };
//            var clock = new FakeClock(default, default);
//            Time = new Time(clock);

//            Space = new Space(info, Options, ParallelOptions, Time);

//            Things = withThings
//                ? AreaData.Select(x =>
//                    {
//                        var thing = CreateThing();
//                        Space.Set(thing, (RectangleF)x[0], null);
//                        return thing;
//                    }).ToArray()
//                : Array.Empty<Thing>();

//            LeaderToken = new();
//            ParallelToken = new(0);
//        }

//        public Space Space { get; }

//        public SpaceOptions Options { get; }

//        public ParallelOptions ParallelOptions { get; }

//        public Time Time { get; }

//        public IEnumerable<Thing> Things { get; }

//        public LeaderToken LeaderToken { get; }

//        public ParallelToken ParallelToken { get; }

//        public BlockIdGroup GetBlockIds(Area area)
//        {
//            area.Intersect(Space.Area);

//            return Rectangle.FromLTRB(
//                (int)MathF.Floor(area.Left / Options.Block.Size.Width),
//                (int)MathF.Floor(area.Top / Options.Block.Size.Height),
//                (int)MathF.Ceiling(area.Right / Options.Block.Size.Width),
//                (int)MathF.Ceiling(area.Bottom / Options.Block.Size.Height));
//        }

//        public Thing CreateThing()
//        {
//            return new Thing(Time);
//        }
//    }
//}
