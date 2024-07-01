using Some1.Play.Core.TestHelpers;
using Some1.Play.Info;
using Some1.Play.Info.TestHelpers;

namespace Some1.Play.Core.Internal;

public class ObjectBoosterGroupTests
{
    [Fact]
    public void Ctor()
    {
        var s = new Services();

        Assert.Equal(s.Infos.ById.Keys, s.Boosters.All.Keys);
    }

    [Fact]
    public void Add()
    {
        var s = new Services();

        {
            s.Boosters.Add(BoosterId.Power, 1);

            Assert.Equal(1, s.Boosters.All[BoosterId.Power].Number);
        }

        {
            s.Boosters.Add(BoosterId.Power, 2);

            Assert.Equal(3, s.Boosters.All[BoosterId.Power].Number);
        }

        {
            s.Boosters.Add(BoosterId.Accel, 1);

            Assert.Equal(1, s.Boosters.All[BoosterId.Accel].Number);
        }

        {
            s.Boosters.Add(BoosterId.Accel, 2);

            Assert.Equal(3, s.Boosters.All[BoosterId.Accel].Number);
        }
    }

    [Fact]
    public void Add_InvalidId_Throws()
    {
        var s = new Services(true);

        Assert.ThrowsAny<Exception>(() => s.Boosters.Add(BoosterId.Power, 1));
    }

    [Fact]
    public void Update()
    {
        var s = new Services();
        s.Boosters.Add(BoosterId.Power, 1);
        s.Boosters.Add(BoosterId.Accel, 1);

        s.Boosters.Update(0.5f);

        Assert.All(s.Boosters.All.Values.Select(x => x.Cycles), x => Assert.Equal(new(0, 0.5f), x));
    }

    [Fact]
    public void Reset()
    {
        var s = new Services();
        s.Boosters.Add(BoosterId.Power, 1);
        s.Boosters.Add(BoosterId.Accel, 1);
        s.Boosters.Update(0.5f);

        s.Boosters.Reset();

        Assert.All(s.Boosters.All.Values, x => Assert.Equal((0, default), (x.Number, x.Cycles)));
    }

    private sealed class Services
    {
        public Services(bool empty = false)
        {
            var info = TestPlayInfoFactory.Create();
            Infos = empty ? new(Enumerable.Empty<BoosterInfo>()) : info.Boosters;
            var clock = new FakeClock();
            var time = new Time(clock);
            var energies = new ObjectEnergyGroup(info.CharacterEnergies);
            var stats = new ObjectStatGroup(info.CharacterStats, energies);
            var stuffs = new ObjectTakeStuffGroup(1, time);
            var properties = new ObjectProperties();

            Boosters = new(Infos, stuffs, stats, properties, time)
            {
                Enabled = true
            };
        }

        public ObjectBoosterGroup Boosters { get; }

        public BoosterInfoGroup Infos { get; }
    }
}
