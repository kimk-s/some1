using System.Drawing;

namespace Some1.Play.Core.Internal;

public class SnapHelperTests
{
    [Fact]
    public void SnapBackX()
    {
        var a= new RectangleF(0, 0, -1, -1);

        {
            var area = SnapHelper.SnapBackX(new RectangleF(1.2f, 1.2f, 1, 1), new(0.1f, 0));
            Assert.Equal(new RectangleF(1, 1.2f, 1, 1), area);
        }

        {
            var area = SnapHelper.SnapBackX(new RectangleF(-1.2f, -1.2f, 1, 1), new(0.1f, 0));
            Assert.Equal(new RectangleF(-2, -1.2f, 1, 1), area);
        }

        {
            var area = SnapHelper.SnapBackX(new RectangleF(1.2f, 1.2f, 1, 1), new(-0.1f, 0));
            Assert.Equal(new RectangleF(2, 1.2f, 1, 1), area);
        }

        {
            var area = SnapHelper.SnapBackX(new RectangleF(-1.2f, -1.2f, 1, 1), new(-0.1f, 0));
            Assert.Equal(new RectangleF(-1, -1.2f, 1, 1), area);
        }
    }

    [Fact]
    public void SnapBackY()
    {
        {
            var area = SnapHelper.SnapBackY(new RectangleF(1.2f, 1.2f, 1, 1), new(0, 0.1f));
            Assert.Equal(new RectangleF(1.2f, 1, 1, 1), area);
        }

        {
            var area = SnapHelper.SnapBackY(new RectangleF(-1.2f, -1.2f, 1, 1), new(0, 0.1f));
            Assert.Equal(new RectangleF(-1.2f, -2, 1, 1), area);
        }

        {
            var area = SnapHelper.SnapBackY(new RectangleF(1.2f, 1.2f, 1, 1), new(0, -0.1f));
            Assert.Equal(new RectangleF(1.2f, 2, 1, 1), area);
        }

        {
            var area = SnapHelper.SnapBackY(new RectangleF(-1.2f, -1.2f, 1, 1), new(0, -0.1f));
            Assert.Equal(new RectangleF(-1.2f, -1, 1, 1), area);
        }
    }

    [Fact]
    public void SnapGoX()
    {
        {
            var area = SnapHelper.SnapGoX(new RectangleF(1.2f, 1.2f, 1, 1), new(0.1f, 0));
            Assert.Equal(new RectangleF(3, 1.2f, 1, 1), area);
        }

        {
            var area = SnapHelper.SnapGoX(new RectangleF(-1.2f, -1.2f, 1, 1), new(0.1f, 0));
            Assert.Equal(new RectangleF(0, -1.2f, 1, 1), area);
        }

        {
            var area = SnapHelper.SnapGoX(new RectangleF(1.2f, 1.2f, 1, 1), new(-0.1f, 0));
            Assert.Equal(new RectangleF(0, 1.2f, 1, 1), area);
        }

        {
            var area = SnapHelper.SnapGoX(new RectangleF(-1.2f, -1.2f, 1, 1), new(-0.1f, 0));
            Assert.Equal(new RectangleF(-3, -1.2f, 1, 1), area);
        }
    }

    [Fact]
    public void SnapGoY()
    {
        {
            var area = SnapHelper.SnapGoY(new RectangleF(1.2f, 1.2f, 1, 1), new(0, 0.1f));
            Assert.Equal(new RectangleF(1.2f, 3, 1, 1), area);
        }

        {
            var area = SnapHelper.SnapGoY(new RectangleF(-1.2f, -1.2f, 1, 1), new(0, 0.1f));
            Assert.Equal(new RectangleF(-1.2f, 0, 1, 1), area);
        }

        {
            var area = SnapHelper.SnapGoY(new RectangleF(1.2f, 1.2f, 1, 1), new(0, -0.1f));
            Assert.Equal(new RectangleF(1.2f, 0, 1, 1), area);
        }

        {
            var area = SnapHelper.SnapGoY(new RectangleF(-1.2f, -1.2f, 1, 1), new(0, -0.1f));
            Assert.Equal(new RectangleF(-1.2f, -3, 1, 1), area);
        }
    }
}
