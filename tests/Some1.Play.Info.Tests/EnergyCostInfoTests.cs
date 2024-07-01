namespace Some1.Play.Info;

public class EnergyCostInfoTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Ctor_ZeroOrNegativeValue_Throws(int value)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new EnergyCostInfo(EnergyId.Health, value));
    }
}
