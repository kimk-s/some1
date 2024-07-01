using Some1.Play.Info;
using Some1.Play.Info.TestHelpers;

namespace Some1.Play.Core.Internal;

public class PlayerSpecialtyTests
{
    [Fact]
    public void Ctor()
    {
        var playInfo = TestPlayInfoFactory.Create();
        var info = playInfo.Specialties.ById[SpecialtyId.Mob1];

        var specialty = new PlayerSpecialty(info);

        Assert.Equal(info.Id, specialty.Id);
        Assert.Equal(0, specialty.Number);
    }
}
