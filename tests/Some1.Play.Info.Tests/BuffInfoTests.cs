namespace Some1.Play.Info;

public class BuffInfoTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Ctor_ZeroOrNegativeCycle_Throws(float cycle)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new BuffInfo(BuffId.Buff1, cycle, false));
    }
}
