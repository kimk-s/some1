using Some1.Play.Core.TestHelpers;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal;

public class TimeTests
{
    [Fact]
    public void Ctor()
    {
        var clock = new FakeClock();

        var time = new Time(clock);

        Assert.Equal(default, time.FrameCount);
        Assert.Equal(0, time.DeltaSeconds);
        Assert.Equal(default, time.TotalSeconds);
        Assert.Equal(default, time.UtcNowSeconds);
        Assert.Equal(default, time.UtcNow);
    }

    [Fact]
    public void Update()
    {
        var s = new Services();

        s.Time.Update(0.5f);
        Assert.Equal(1, s.Time.FrameCount);
        Assert.Equal(0.5f, s.Time.DeltaSeconds);
        Assert.Equal(0.5f, s.Time.TotalSeconds);
        Assert.Equal(0.5d, s.Time.UtcNowSeconds);
        Assert.Equal(PlayConst.StandardDateTime.AddSeconds(s.Time.UtcNowSeconds), s.Time.UtcNow);

        s.Time.Update(0.5f);
        Assert.Equal(2, s.Time.FrameCount);
        Assert.Equal(0.5f, s.Time.DeltaSeconds);
        Assert.Equal(1, s.Time.TotalSeconds);
        Assert.Equal(1, s.Time.UtcNowSeconds);
        Assert.Equal(PlayConst.StandardDateTime.AddSeconds(s.Time.UtcNowSeconds), s.Time.UtcNow);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Update_InvalidDeltaSeconds_Throws(float deltaSeconds)
    {
        var s = new Services();

        Assert.Throws<ArgumentOutOfRangeException>(() => s.Time.Update(deltaSeconds));
    }

    private sealed class Services
    {
        public Services()
        {
            Clock = new FakeClock(new(2020, 1, 1), TimeSpan.FromSeconds(ClockUtcNowDelta));

            Time = new Time(Clock);
        }

        public Time Time { get; }

        public FakeClock Clock { get; }

        public float ClockUtcNowDelta { get; } = 0.5f;
    }
}
