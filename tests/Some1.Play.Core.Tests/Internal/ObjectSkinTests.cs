using Some1.Play.Info;

namespace Some1.Play.Core.Internal;

public class ObjectSkinTests
{
    [Fact]
    public void Ctor()
    {
        var s = new Services();

        Assert.Null(s.Skin.Id);
        Assert.False(s.Skin.Value);
    }

    [Theory]
    [InlineData(SkinId.Skin0)]
    [InlineData(SkinId.Skin1)]
    public void SetId(SkinId id)
    {
        var s = new Services();

        s.Skin.Set(id);

        Assert.Equal(id, s.Skin.Id);
        Assert.False(s.Skin.Value);
    }

    [Fact]
    public void SetValue()
    {
        var s = new Services();

        {
            s.Skin.Set(SkinId.Skin0);
            s.Skin.Set(CharacterId.Player1);

            Assert.True(s.Skin.Value);
        }

        {
            s.Skin.Set(SkinId.Skin1);
            s.Skin.Set(CharacterId.Player1);

            Assert.True(s.Skin.Value);
        }

        {
            s.Skin.Set(SkinId.Skin0);
            s.Skin.Set(CharacterId.Player2);

            Assert.False(s.Skin.Value);
        }

        {
            s.Skin.Set(SkinId.Skin1);
            s.Skin.Set(CharacterId.Player2);

            Assert.False(s.Skin.Value);
        }
    }

    [Fact]
    public void Reset()
    {
        var s = new Services();
        s.Skin.Set(SkinId.Skin0);
        s.Skin.Set(CharacterId.Player1);

        s.Skin.Reset();

        Assert.Null(s.Skin.Id);
        Assert.False(s.Skin.Value);
    }

    private sealed class Services
    {
        public Services()
        {
            var infos = new CharacterSkinInfoGroup(new CharacterSkinInfo[]
            {
                new(new(CharacterId.Player1, SkinId.Skin0))
            });
            var properties = new ObjectProperties();

            Skin = new ObjectSkin(infos, properties);
        }

        public ObjectSkin Skin { get; }
    }
}
