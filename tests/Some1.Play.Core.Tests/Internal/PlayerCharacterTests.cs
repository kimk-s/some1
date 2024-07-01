using Some1.Play.Info;
using Some1.Play.Info.TestHelpers;

namespace Some1.Play.Core.Internal;

public class PlayerCharacterTests
{
    [Fact]
    public void Ctor()
    {
        var playInfo = TestPlayInfoFactory.Create();

        var character = new PlayerCharacter(
            playInfo.Characters.ById[CharacterId.Player1],
            playInfo.CharacterSkins.GroupByCharacterId[CharacterId.Player1]);

        Assert.Equal(CharacterId.Player1, character.Id);
        Assert.False(character.IsSelected);
    }
}
