using Some1.Play.Core.TestHelpers;
using Some1.Play.Info;
using Some1.Play.Info.TestHelpers;

namespace Some1.Play.Core.Internal;

public class ObjectBoosterTests
{
    [Fact]
    public void Ctor()
    {
        var s = new Services();

        Assert.Equal(BoosterId.Power, s.Booster.Id);
        Assert.Equal(0, s.Booster.Number);
        Assert.Equal(default, s.Booster.Cycles);
    }

    [Fact]
    public void Add()
    {
        var s = new Services();

        {
            s.Booster.Add(1);

            Assert.Equal(1, s.Booster.Number);
            Assert.Equal(default, s.Booster.Cycles);
        }

        {
            s.Booster.Add(2);

            Assert.Equal(3, s.Booster.Number);
            Assert.Equal(default, s.Booster.Cycles);
        }

        {
            s.Booster.Add(-1);

            Assert.Equal(2, s.Booster.Number);
            Assert.Equal(default, s.Booster.Cycles);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(PlayConst.MaxBoosterNumber + 1)]
    public void Add_OverBoosterNumber_Clamp(int number)
    {
        var s = new Services();

        s.Booster.Add(number);

        Assert.Equal(Math.Clamp(number, 0, PlayConst.MaxBoosterNumber), s.Booster.Number);
        Assert.Equal(default, s.Booster.Cycles);
    }

    [Fact]
    public void Add_ZeroBoosterNumber_Reset()
    {
        var s = new Services();
        s.Booster.Add(1);
        s.Booster.Update(0.5f);

        s.Booster.Add(-1);

        Assert.Equal(0, s.Booster.Number);
        Assert.Equal(default, s.Booster.Cycles);
    }

    [Fact]
    public void Add_ZeroNumber_Throws()
    {
        var s = new Services();

        Assert.Throws<ArgumentOutOfRangeException>(() => s.Booster.Add(0));
    }

    [Fact]
    public void Update_ZeroBoosterNumber()
    {
        var s = new Services();

        s.Booster.Update(0.5f);

        Assert.Equal(default, s.Booster.Cycles);
    }

    [Fact]
    public void Update()
    {
        var s = new Services();
        s.Booster.Add(2);

        {
            s.Booster.Update(1);

            Assert.Equal(2, s.Booster.Number);
            Assert.Equal(new(0, 1), s.Booster.Cycles);
        }

        {
            s.Booster.Update(1);

            Assert.Equal(1, s.Booster.Number);
            Assert.Equal(new(1, 2), s.Booster.Cycles);
        }

        {
            s.Booster.Update(1);

            Assert.Equal(0, s.Booster.Number);
            Assert.Equal(default, s.Booster.Cycles);
        }
    }

    [Fact]
    public void Reset()
    {
        var s = new Services();
        s.Booster.Add(1);
        s.Booster.Update(0.5f);

        s.Booster.Reset();

        Assert.Equal(0, s.Booster.Number);
        Assert.Equal(default, s.Booster.Cycles);
    }

    private sealed class Services
    {
        public Services() : this(BoosterId.Power, 1f)
        {
        }

        public Services(BoosterId id, float seconds)
        {
            var info = TestPlayInfoFactory.Create();
            var clock = new FakeClock();
            var time = new Time(clock);
            var energies = new ObjectEnergyGroup(info.CharacterEnergies);
            var stats = new ObjectStatGroup(info.CharacterStats, energies);

            Booster = new(new BoosterInfo(id, seconds), stats, time);
        }

        public ObjectBooster Booster { get; }
    }
}
