namespace Some1.Play.Info;

public class CharacterWalkInfoTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Ctor_ZeroOrLessZeroSpeed(float speed)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CharacterWalkInfo(speed, 1));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Ctor_ZeroOrLessZeroSeconds(float seconds)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CharacterWalkInfo(1, seconds));
    }
}
