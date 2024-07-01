namespace Some1.Play.Info;

public class CycleRepeatTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(-2)]
    public void Ctor_NegativeStart_Throws(float start)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CycleRepeat(start, 1f));
    }
}
