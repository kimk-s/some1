namespace Some1.Play.Core;

public class BlockIdGroupTests
{
    [Theory]
    [InlineData(1, 0, 0, 0)]
    [InlineData(0, 1, 0, 0)]
    [InlineData(0, 0, 1, 0)]
    [InlineData(0, 0, 0, 1)]
    public void Ctor(int x, int y, int w, int h)
    {
        var blockId = new BlockIdGroup(x, y, w, h);

        Assert.Equal(x, blockId.X);
        Assert.Equal(y, blockId.Y);
        Assert.Equal(w, blockId.Width);
        Assert.Equal(h, blockId.Height);
    }

    [Theory]
    [InlineData(-1, 0, 0, 0)]
    [InlineData(0, -1, 0, 0)]
    [InlineData(0, 0, -1, 0)]
    [InlineData(0, 0, 0, -1)]
    public void Ctor_NegativeArguments_Throws(int x, int y, int w, int h)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new BlockIdGroup(x, y, w, h));
    }
}
