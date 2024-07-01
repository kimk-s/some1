using System.Drawing;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal;

public class LeaderTokenTests
{
    [Fact]
    public void Ctor()
    {
        var token = new LeaderToken();
        Assert.Equal(default, token.Area);
    }

    [Fact]
    public void SetArea()
    {
        var token = new LeaderToken();

        token.SetArea(Area.Rectangle(new PointF(1, 1), new SizeF(1, 1)));

        Assert.Equal(Area.Rectangle(new PointF(1, 1), new SizeF(1, 1)), token.Area);
    }

    [Fact]
    public void ResetArea()
    {
        var token = new LeaderToken();
        token.SetArea(Area.Rectangle(new PointF(1, 1), new SizeF(1, 1)));

        token.ResetArea();

        Assert.Equal(default, token.Area);
    }
}
