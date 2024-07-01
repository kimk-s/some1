namespace Some1.Play.Core.Paralleling;

public class ParallelTokenTests
{
    [Fact]
    public void Ctor()
    {
        ParallelToken token;

        token = new ParallelToken(0);
        Assert.Equal(0, token.Index);

        token = new ParallelToken(1);
        Assert.Equal(1, token.Index);
    }

    [Fact]
    public void Ctor_NegativeIndex_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ParallelToken(-1));
    }
}
