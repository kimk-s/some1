using System.Drawing;
using System.Numerics;
using Some1.Play.Core.TestHelpers;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal;

public class ObjectTransferTests
{
    [Fact]
    public void Set()
    {
        var s = new Services();
        s.Transfer.MoveDelta = new(1, 1);

        s.Transfer.Set(new Vector2(1, 1), null);

        Assert.True(s.Transfer.IsMoveBlocked);
        Assert.Equal(Vector2.Zero, s.Transfer.MoveDelta);
        Assert.Equal(Area.Rectangle(new Vector2(1, 1), new SizeF(1, 1)), s.Object.Properties.Area);
    }

    [Fact]
    public void Teleport()
    {
        var s = new Services();
        s.Transfer.MoveDelta = new(1, 1);
        s.Transfer.Set(new Vector2(0.5f, 0.5f), null);

        s.Transfer.Teleport(new(1, 1), null);

        Assert.True(s.Transfer.IsMoveBlocked);
        Assert.Equal(Vector2.Zero, s.Transfer.MoveDelta);
        Assert.Equal(new RectangleF(1, 1, 1, 1), s.Object.Properties.Area);
    }

    [Fact]
    public void AddMoveDelta()
    {
        var s = new Services();

        s.Transfer.MoveDelta = new Vector2(1, 1);

        Assert.False(s.Transfer.IsMoveBlocked);
        Assert.Equal(new(1, 1), s.Transfer.MoveDelta);
    }

    [Fact]
    public void AddMoveDelta_WhenIsMoveBlocked()
    {
        var s = new Services();
        s.Transfer.Set(new Vector2(1, 1), null);

        s.Transfer.MoveDelta = new Vector2(1, 1);

        Assert.True(s.Transfer.IsMoveBlocked);
        Assert.Equal(Vector2.Zero, s.Transfer.MoveDelta);
    }

    [Fact]
    public void Update_WhenIsMoveBlocked()
    {
        var s = new Services();
        s.Transfer.Set(new Vector2(1, 1), null);

        s.Transfer.Update(new(0));

        Assert.False(s.Transfer.IsMoveBlocked);
        Assert.Equal(Vector2.Zero, s.Transfer.MoveDelta);
        Assert.Equal(Area.Rectangle(new Vector2(1, 1), new SizeF(1, 1)), s.Object.Properties.Area);
    }

    [Fact]
    public void Update_WhenIsMoveNotBlocked()
    {
        var s = new Services();
        s.Transfer.Set(new Vector2(0.5f, 0.5f), null);
        s.Transfer.Update(new(0));
        s.Transfer.MoveDelta = new(1, 1);

        s.Transfer.Update(new(0));

        Assert.False(s.Transfer.IsMoveBlocked);
        Assert.Equal(Vector2.Zero, s.Transfer.MoveDelta);
        Assert.Equal(Area.Rectangle(new Vector2(1.5f, 1.5f), new SizeF(1, 1)), s.Object.Properties.Area);
    }

    [Fact]
    public void Reset()
    {
        var s = new Services();
        s.Transfer.Set(new Vector2(1, 1), null);

        s.Transfer.Reset(null);

        Assert.False(s.Transfer.IsMoveBlocked);
        Assert.Equal(Vector2.Zero, s.Transfer.MoveDelta);
        Assert.Equal(Area.Rectangle(Vector2.Zero, new SizeF(1, 1)), s.Object.Properties.Area);
    }

    private sealed class Services
    {
        private readonly ObjectFactoryServices _ofs = new();

        public Services()
        {
            Object = _ofs.ObjectFactory.CreateRoot(true);
            Object.SetCharacter(CharacterId.Player1);
            Object.SetBattle(true);
            Transfer = (ObjectTransfer)Object.Transfer;
        }

        public ObjectTransfer Transfer { get; }

        public Object Object { get; }

        public Space Space => _ofs.Space;
    }
}
