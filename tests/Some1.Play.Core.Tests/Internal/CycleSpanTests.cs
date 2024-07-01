using Some1.Play.Info;

namespace Some1.Play.Core.Internal;

public class CycleSpanTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(0, 1)]
    public void Ctor(float a, float b)
    {
        var span = new CycleSpan(a, b);

        Assert.Equal(a, span.A);
        Assert.Equal(b, span.B);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(1, 0)]
    public void Ctor_InvalidArgument_Throws(float a, float b)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CycleSpan(a, b));
    }

    [Theory]
    [InlineData(-1)]
    public void Contains_InvalidArgument_Throws(float start)
    {
        var span = new CycleSpan(0, 1);

        Assert.Throws<ArgumentOutOfRangeException>(() => span.Contains(start, 1));
    }

    [Theory]
    [InlineData(0f, 0f)]
    [InlineData(0.5f, 0.5f)]
    [InlineData(1f, 1f)]
    public void Contains_WhenAAndBIsEquals(float a, float b)
    {
        var span = new CycleSpan(a, b);

        Assert.Equal(0, span.Contains(0, 1f));
        Assert.Equal(0, span.Contains(0.5f, 1f));
        Assert.Equal(0, span.Contains(1f, 1f));
    }

    [Theory]
    [InlineData(0.5f, 0.5f)]
    [InlineData(1f, 1f)]
    [InlineData(1f, 0.5f)]
    [InlineData(1.5f, 1f)]
    public void Contains_StartIsEqualsOrGreaterThenB(float start, float b)
    {
        var span = new CycleSpan(0f, b);

        Assert.Equal(0, span.Contains(start, 2f));
        Assert.Equal(0, span.Contains(start, 1f));
        Assert.Equal(0, span.Contains(start, 0.5f));
        Assert.Equal(0, span.Contains(start, 0.3f));
    }

    [Fact]
    public void Contains_StartIsInAAndB()
    {
        var span = new CycleSpan(0, 1);

        Assert.Equal(1, span.Contains(0f, 2f));
        Assert.Equal(1, span.Contains(0.3f, 2f));
        Assert.Equal(1, span.Contains(0.5f, 2f));
        Assert.Equal(1, span.Contains(0.6f, 2f));

        Assert.Equal(1, span.Contains(0.9f, 1f));
        Assert.Equal(1, span.Contains(0f, 1f));
        Assert.Equal(1, span.Contains(0.3f, 1f));
        Assert.Equal(1, span.Contains(0.5f, 1f));
        Assert.Equal(1, span.Contains(0.6f, 1f));
        Assert.Equal(1, span.Contains(0.9f, 1f));

        Assert.Equal(2, span.Contains(0f, 0.5f));
        Assert.Equal(2, span.Contains(0.3f, 0.5f));
        Assert.Equal(1, span.Contains(0.5f, 0.5f));
        Assert.Equal(1, span.Contains(0.6f, 0.5f));
        Assert.Equal(1, span.Contains(0.9f, 0.5f));

        Assert.Equal(4, span.Contains(0f, 0.3f));
        Assert.Equal(3, span.Contains(0.3f, 0.3f));
        Assert.Equal(2, span.Contains(0.5f, 0.3f));
        Assert.Equal(2, span.Contains(0.6f, 0.3f));
        Assert.Equal(1, span.Contains(0.9f, 0.3f));
    }

    [Fact]
    public void Contains_StartIsLessThanA()
    {
        var span = new CycleSpan(1, 2);

        Assert.Equal(0, span.Contains(0f, 2f));
        Assert.Equal(0, span.Contains(0.3f, 2f));
        Assert.Equal(0, span.Contains(0.5f, 2f));
        Assert.Equal(0, span.Contains(0.6f, 2f));
        Assert.Equal(0, span.Contains(0.9f, 2f));

        Assert.Equal(1, span.Contains(0f, 1f));
        Assert.Equal(1, span.Contains(0.3f, 1f));
        Assert.Equal(1, span.Contains(0.5f, 1f));
        Assert.Equal(1, span.Contains(0.6f, 1f));
        Assert.Equal(1, span.Contains(0.9f, 1f));

        Assert.Equal(2, span.Contains(0f, 0.5f));
        Assert.Equal(2, span.Contains(0.3f, 0.5f));
        Assert.Equal(2, span.Contains(0.5f, 0.5f));
        Assert.Equal(2, span.Contains(0.6f, 0.5f));
        Assert.Equal(2, span.Contains(0.9f, 0.5f));

        Assert.Equal(3, span.Contains(0f, 0.3f));
        Assert.Equal(3, span.Contains(0.3f, 0.3f));
        Assert.Equal(3, span.Contains(0.5f, 0.3f));
        Assert.Equal(3, span.Contains(0.6f, 0.3f));
        Assert.Equal(3, span.Contains(0.9f, 0.3f));
    }

    [Fact]
    public void Contains_EveryFrameInterval()
    {
        var span = new CycleSpan(1, 2);

        Assert.Equal(1, span.Contains(0, CycleRepeat.EveryFrameInterval));
        Assert.Equal(1, span.Contains(0.5f, CycleRepeat.EveryFrameInterval));
        Assert.Equal(1, span.Contains(1f, CycleRepeat.EveryFrameInterval));
        Assert.Equal(1, span.Contains(1.5f, CycleRepeat.EveryFrameInterval));
    }

    [Fact]
    public void Contains_OutStartAndEveryFrameInterval()
    {
        var span = new CycleSpan(1, 2);

        Assert.Equal(0, span.Contains(2f, CycleRepeat.EveryFrameInterval));
        Assert.Equal(0, span.Contains(2.5f, CycleRepeat.EveryFrameInterval));
    }

    [Fact]
    public void Contains_NoRepeatInterval()
    {
        var span = new CycleSpan(1, 2);

        Assert.Equal(1, span.Contains(1f, CycleRepeat.NoRepeatInterval));
        Assert.Equal(1, span.Contains(1.5f, CycleRepeat.NoRepeatInterval));
    }

    [Fact]
    public void Contains_OutStartAndNoRepeatInterval()
    {
        var span = new CycleSpan(1, 2);

        Assert.Equal(0, span.Contains(0f, CycleRepeat.NoRepeatInterval));
        Assert.Equal(0, span.Contains(0.5f, CycleRepeat.NoRepeatInterval));
        Assert.Equal(0, span.Contains(2f, CycleRepeat.NoRepeatInterval));
    }
}
