using System.Drawing;
using Some1.Play.Core.Paralleling;
using Some1.Play.Core.TestHelpers;
using Some1.Play.Info;
using ParallelOptions = Some1.Play.Core.Paralleling.ParallelOptions;

namespace Some1.Play.Core.Internal;

public class BlockTests
{
    [Fact]
    public void Ctor()
    {
        var id = new BlockId(1, 1);
        var area = Area.Rectangle(new PointF(1, 1), 1);
        var parallelOptions = new ParallelOptions { Count = 1 };
        var clock = new FakeClock();
        var time = new Time(clock);

        var block = new TestBlock(id, area, parallelOptions, time);

        Assert.Equal(id, block.Id);
        Assert.Equal(area, block.Area);
        Assert.Empty(block.Items);
        Assert.False(block.Control.IsTaken);
        Assert.Empty(block.Messages);
        Assert.Equal(parallelOptions.Count, block.Messages.ParallelCount);
    }

    [Fact]
    public void Add_NullParallelToken()
    {
        var s = new Services(new(1, 1));
        var itemValue = new TestBlockItemValue(new(1), new(1, 1, 1, 1));

        s.Block.Add(itemValue, null);

        Assert.Equal(itemValue, s.Block.Items.Single().Value);
    }

    [Fact]
    public void Add_ParallelToken()
    {
        var s = new Services(new(1, 1));
        var itemValue = new TestBlockItemValue(new(1), new(1, 1, 1, 1));

        s.Block.Add(itemValue, s.ParallelToken);

        Assert.Empty(s.Block.Items);
        Assert.Equal(new(true, itemValue), s.Block.Messages.Single());
    }

    [Fact]
    public void Add_NullObject_Throws()
    {
        var s = new Services();

        Assert.Throws<ArgumentNullException>(() => s.Block.Add(null!, null));
    }

    [Fact]
    public void Add_WhenDuplicated_ThenIgnore()
    {
        var s = new Services(new(1, 1));
        var itemValue = new TestBlockItemValue(new(1), new(1, 1, 1, 1));

        s.Block.Add(itemValue, null);
        s.Block.Add(itemValue, null);

        Assert.Equal(itemValue, s.Block.Items.Single().Value);
    }

    [Theory]
    [InlineData(0, 0, 1, 1)]
    [InlineData(0, 1, 1, 0)]
    [InlineData(1, 0, 0, 1)]
    [InlineData(1, 1, 0, 0)]
    public void Add_WhenBlockIdIsNotContaines_ThenIgnore(int a, int b, int x, int y)
    {
        var s = new Services(new(a, b));
        var itemValue = new TestBlockItemValue(new(1), new(x, y, 1, 1));

        s.Block.Add(itemValue, null);

        Assert.Empty(s.Block.Items);
    }

    [Fact]
    public void Remove_NullParallelToken()
    {
        var s = new Services(new(1, 1));
        var itemValue = new TestBlockItemValue(new(1), new(1, 1, 1, 1));
        s.Block.Add(itemValue, null);

        itemValue.BlockIds = default;
        s.Block.Remove(itemValue, null);

        Assert.Empty(s.Block.Items);
    }

    [Fact]
    public void Remove_ParallelToken()
    {
        var s = new Services(new(1, 1));
        var itemValue = new TestBlockItemValue(new(1), new(1, 1, 1, 1));

        s.Block.Remove(itemValue, s.ParallelToken);

        Assert.Equal(new(false, itemValue), s.Block.Messages.Single());
    }

    [Fact]
    public void Remove_NullObject_Throws()
    {
        var s = new Services();

        Assert.Throws<ArgumentNullException>(() => s.Block.Remove(null!, null));
    }

    [Fact]
    public void Remove_WhenNotExists_ThenIgnore()
    {
        var s = new Services(new(1, 1));
        var itemValue = new TestBlockItemValue(new(1), new(1, 1, 1, 1));

        s.Block.Remove(itemValue, null);

        Assert.Empty(s.Block.Items);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(1, 1)]
    public void Remove_WhenBlockIdIsContaines_ThenIgnore(int a, int b)
    {
        var s = new Services(new(a, b));
        var itemValue = new TestBlockItemValue(new(1), new(a, b, 1, 1));
        s.Block.Add(itemValue, null);

        s.Block.Remove(itemValue, null);

        Assert.Equal(itemValue, s.Block.Items.Single().Value);
    }

    [Fact]
    public void TryUpdate_WithAdd()
    {
        var s = new Services();
        s.Time.Update(1f);
        var itemValue = s.CreateItemValue();
        s.Block.Add(itemValue, s.ParallelToken);

        var isUpdated = s.Block.TryUpdate();

        Assert.True(isUpdated);
        Assert.Equal(itemValue, s.Block.Items.Single().Value);
        Assert.True(s.Block.Control.IsTaken);
        Assert.Empty(s.Block.Messages);
    }

    [Fact]
    public void TryUpdate_WithRemove()
    {
        var s = new Services();
        s.Time.Update(1f);
        var itemValue = s.CreateItemValue();
        s.Block.Add(itemValue, null);
        itemValue.BlockIds = default;
        s.Block.Remove(itemValue, s.ParallelToken);

        var isUpdated = s.Block.TryUpdate();

        Assert.True(isUpdated);
        Assert.Empty(s.Block.Items);
        Assert.True(s.Block.Control.IsTaken);
        Assert.Empty(s.Block.Messages);
    }

    private sealed class Services
    {
        private readonly ParallelOptions _parallelOptions;

        public Services() : this(new(1, 1))
        {

        }

        public Services(BlockId id)
        {
            var area = Area.Rectangle(new PointF(1, 1), 1);
            _parallelOptions = new ParallelOptions { Count = 1 };
            var clock = new FakeClock();
            Time = new Time(clock);
            ParallelToken = new(0);

            Block = new TestBlock(id, area, _parallelOptions, Time);
        }

        public TestBlock Block { get; }

        public Time Time { get; }

        public ParallelToken ParallelToken { get; }

        public TestBlockItemValue CreateItemValue(int id = 1)
        {
            return new TestBlockItemValue(new(1), new(Block.Id, new(1, 1)));
        }
    }

    private sealed class TestBlock : Block<TestBlockItemValue, TestBlockItemStatic>
    {
        internal TestBlock(BlockId id, Area area, ParallelOptions parallelOptions, ITime time) : base(id, area, parallelOptions, time)
        {
        }
    }

    private sealed class TestBlockItemValue : IBlockItemValue<TestBlockItemStatic>
    {
        public TestBlockItemValue(TestBlockItemStatic @static, BlockIdGroup blockIds)
        {
            Static = @static;
            Id = @static.Id;
            BlockIds = blockIds;
        }

        public TestBlockItemStatic Static { get; }

        public int Id { get; }

        public BlockIdGroup BlockIds { get; set; }
    }

    private readonly struct TestBlockItemStatic : IBlockItemStatic
    {
        public TestBlockItemStatic(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
