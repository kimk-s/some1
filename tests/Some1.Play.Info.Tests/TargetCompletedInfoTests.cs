namespace Some1.Play.Info;

public class TargetCompletedInfoTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(-2)]
    public void Ctor_NegativeHealthDecreasement_Throws(int healthDecreasement)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new TargetCompletedInfo(false, healthDecreasement));
    }
}
