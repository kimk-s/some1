using System.Drawing;

namespace Some1.Play.Info;

public class AreaTests
{
    [Fact]
    public void Rectangle()
    {
        var area = Area.Rectangle(new PointF(1, 2), new SizeF(3, 4));

        Assert.Equal(AreaType.Rectangle, area.Type);
        Assert.Equal(new(1, 2, 3, 4), area.Data);
    }

    [Fact]
    public void Circle()
    {
        var area = Area.Circle(new PointF(1, 2), 3);

        Assert.Equal(AreaType.Circle, area.Type);
        Assert.Equal(new(1, 2, 3, 3), area.Data);
    }

    [Theory]
    [InlineData(0, 0, 1, 1, 0, 0, 1, 1)]
    [InlineData(0, 0, 2, 2, 0, 0, 1, 1)]
    [InlineData(0, 0, 2, 2, 1, 1, 1, 1)]
    public void IntersectsWith_RectangleAndRectangle_True(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
    {
        var result = Area.Rectangle(new PointF(x1, y1), new SizeF(w1, h1))
            .IntersectsWith(Area.Rectangle(new PointF(x2, y2), new SizeF(w2, h2)));

        Assert.True(result);
    }

    [Theory]
    [InlineData(0, 0, 0, 0, 0, 0, 0, 0)]
    [InlineData(0, 0, 1, 1, 1, 1, 1, 1)]
    public void IntersectsWith_RectangleAndRectangle_False(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
    {
        var result = Area.Rectangle(new PointF(x1, y1), new SizeF(w1, h1))
            .IntersectsWith(Area.Rectangle(new PointF(x2, y2), new SizeF(w2, h2)));

        Assert.False(result);
    }

    [Theory]
    [InlineData(0, 0, 1, 1, 0, 0, 1)]
    public void IntersectsWith_RectangleAndCircle_True(float x1, float y1, float w, float h, float x2, float y2, float d)
    {
        var result = Area.Rectangle(new PointF(x1, y1), new SizeF(w, h))
            .IntersectsWith(Area.Circle(new PointF(x2, y2), d));

        Assert.True(result);
    }

    [Theory]
    [InlineData(0, 0, 0, 0, 0, 0, 0)]
    [InlineData(0, 0, 1, 1, 1, 1, 1)]
    public void IntersectsWith_RectangleAndCircle_False(float x1, float y1, float w, float h, float x2, float y2, float d)
    {
        var result = Area.Rectangle(new PointF(x1, y1), new SizeF(w, h))
            .IntersectsWith(Area.Circle(new PointF(x2, y2), d));

        Assert.False(result);
    }

    [Theory]
    [InlineData(0, 0, 1, 0, 0, 1)]
    public void IntersectsWith_CircleAndCircle_True(float x1, float y1, float d1, float x2, float y2, float d2)
    {
        var result = Area.Circle(new PointF(x1, y1), d1)
            .IntersectsWith(Area.Circle(new PointF(x2, y2), d2));

        Assert.True(result);
    }

    [Theory]
    [InlineData(0, 0, 0, 0, 0, 0)]
    public void IntersectsWith_CircleAndCircle_False(float x1, float y1, float d1, float x2, float y2, float d2)
    {
        var result = Area.Circle(new PointF(x1, y1), d1)
            .IntersectsWith(Area.Circle(new PointF(x2, y2), d2));

        Assert.False(result);
    }

    [Fact]
    public void ImplicitOperatorArea()
    {
        Area area = new RectangleF(1, 1, 1, 1);

        Assert.Equal(AreaType.Rectangle, area.Type);
        Assert.Equal(new(1, 1, 1, 1), area.Data);
    }

    [Fact]
    public void ExplicitOperatorRectangle_RectangleAreaType()
    {
        var rect = (RectangleF)Area.Rectangle(new PointF(1, 1), new SizeF(1, 1));

        Assert.Equal(new(1, 1, 1, 1), rect);
    }

    [Fact]
    public void ExplicitOperatorRectangle_CircleAreaType_Throws()
    {
        Assert.Throws<InvalidOperationException>(() => (RectangleF)Area.Circle(new PointF(1, 1), 1));
    }
}
