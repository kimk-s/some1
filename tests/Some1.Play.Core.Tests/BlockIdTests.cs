namespace Some1.Play.Core;

public class BlockIdTests
{
    [Theory]
    [InlineData(1, 0)]
    [InlineData(0, 1)]
    public void Ctor(int x, int y)
    {
        var blockId = new BlockId(x, y);

        Assert.Equal(x, blockId.X);
        Assert.Equal(y, blockId.Y);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    public void Ctor_NegativeArguments_Throws(int x, int y)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new BlockId(x, y));
    }
}
