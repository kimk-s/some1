namespace Some1.Play.Info;

public class CharacterCastInfoTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Ctor_ZeroOrNegativeCycle_Throws(float cycle)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new CharacterCastInfo(
                new(CharacterId.Player1, CastId.Attack),
                cycle,
                1,
                1,
                true,
                default,
                default));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Ctor_ZeroOrNegativeLoadTime_Throws(float loadTime)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new CharacterCastInfo(
                new(CharacterId.Player1, CastId.Attack),
                1,
                loadTime,
                1,
                true,
                default,
                default));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Ctor_ZeroOrNegativeLoadCount_Throws(int loadCount)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new CharacterCastInfo(
                new(CharacterId.Player1, CastId.Attack),
                1,
                1,
                loadCount,
                true,
                default,
                default));
    }
}
