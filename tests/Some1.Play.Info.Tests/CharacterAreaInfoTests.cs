namespace Some1.Play.Info;

public class CharacterAreaInfoTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void Ctor(float size)
    {
        var info = new CharacterAreaInfo(default, size);

        Assert.Equal(size, info.Size);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Ctor_ZeroOrNegativeArguments_Throws(float size)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CharacterAreaInfo(default, size));
    }
}
