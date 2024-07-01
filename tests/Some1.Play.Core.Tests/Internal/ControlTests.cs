using Some1.Play.Core.TestHelpers;

namespace Some1.Play.Core.Internal;

public class ControlTests
{
    [Fact]
    public void Ctor_Default()
    {
        var clock = new FakeClock();
        var time = new Time(clock);

        var control = new Control(time);

        Assert.Equal(0, control.TakenFrameCount);
        Assert.False(control.CanTake);
        Assert.False(control.IsTaken);
    }

    [Fact]
    public void Ctor_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new Control(null!));
    }

    [Fact]
    public void TryTake()
    {
        var s = new Services();
        s.Time.Update(1f);

        var isTaken = s.Control.TryTake();

        Assert.True(isTaken);
        Assert.Equal(s.Time.FrameCount, s.Control.TakenFrameCount);
        Assert.False(s.Control.CanTake);
        Assert.True(s.Control.IsTaken);
    }

    [Fact]
    public void TryTake_CanNotTake_False()
    {
        var s = new Services();

        var isTaken = s.Control.TryTake();

        Assert.False(isTaken);
        Assert.Equal(0, s.Control.TakenFrameCount);
        Assert.False(s.Control.CanTake);
        Assert.False(s.Control.IsTaken);
    }

    [Fact]
    public void Reset()
    {
        var s = new Services();
        s.Time.Update(1f);
        _ = s.Control.TryTake();

        s.Control.Reset();

        Assert.Equal(0, s.Control.TakenFrameCount);
        Assert.True(s.Control.CanTake);
        Assert.False(s.Control.IsTaken);
    }

    private sealed class Services
    {
        public Services()
        {
            var clock = new FakeClock();
            Time = new Time(clock);
            Control = new(Time);
        }

        public Control Control { get; }

        public Time Time { get; }
    }
}
