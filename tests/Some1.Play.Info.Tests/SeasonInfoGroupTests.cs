namespace Some1.Play.Info;

public class SeasonInfoGroupTests
{
    [Fact]
    public void Ctor_CurrentNotSingle_Throws()
    {
        Assert.ThrowsAny<Exception>(() => new SeasonInfoGroup(new SeasonInfo[]
        {
            new SeasonInfo(SeasonId.Season1, SeasonType.Past),
            new SeasonInfo(SeasonId.Season2, SeasonType.ComingSoon),
        }));

        Assert.ThrowsAny<Exception>(() => new SeasonInfoGroup(new SeasonInfo[]
        {
            new SeasonInfo(SeasonId.Season1, SeasonType.Current),
            new SeasonInfo(SeasonId.Season2, SeasonType.Current),
        }));
    }
}
