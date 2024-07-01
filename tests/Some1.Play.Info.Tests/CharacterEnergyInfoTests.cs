namespace Some1.Play.Info;

public class CharacterEnergyInfoTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(-2)]
    public void Ctor_NegativeValue_Throws(int value)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new CharacterEnergyInfo(new(CharacterId.Player1, EnergyId.Health), value));
    }
}
