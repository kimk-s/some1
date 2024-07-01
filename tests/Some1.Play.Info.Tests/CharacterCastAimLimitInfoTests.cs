namespace Some1.Play.Info;

public class CharacterCastAimLimitInfoTests
{
    [Fact]
    public void Ctor_MinDistanceGreaterThanMaxDistance_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new CharacterCastAimLimitInfo(true, 1, 0));
    }
}
