namespace Some1.Play.Core.Paralleling;

public class ParallelTokenGroupTests
{
    [Fact]
    public void Ctor()
    {
        ParallelTokenGroup tokens;

        tokens = new(new() { Count = 1 });
        Assert.Equal(Enumerable.Range(0, 1), tokens.All.Select(x => x.Index));

        tokens = new(new() { Count = 2 });
        Assert.Equal(Enumerable.Range(0, 2), tokens.All.Select(x => x.Index));
    }

    [Fact]
    public void Get()
    {
        ParallelTokenGroup tokens = new(new() { Count = 2 });
        ParallelToken token;

        token = tokens.All[0];
        Assert.Equal(0, token.Index);

        token = tokens.All[1];
        Assert.Equal(1, token.Index);
    }
}
