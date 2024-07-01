using Some1.Play.Core.TestHelpers;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal;

public class ObjectHitTests
{
    [Fact]
    public void Ctor()
    {
        var s = new Services();

        Assert.Null(s.Hit.Id);
        Assert.Equal(0, s.Hit.Value);
        Assert.Equal(0, s.Hit.RootId);
        Assert.Equal(0, s.Hit.Token);
        Assert.Equal(default, s.Hit.Cycles);
    }

    [Fact]
    public void SetDamage()
    {
        var s = new Services();

        s.Hit.Set(HitId.Default, 1, 0, 0, default, 1);
        Assert.Equal(HitId.Default, s.Hit.Id);
        Assert.Equal(1, s.Hit.Value);
        Assert.Equal(0, s.Hit.RootId);
        Assert.Equal(1, s.Hit.Token);
        Assert.Equal(default, s.Hit.Cycles);
        Assert.Equal(9, s.Energies.All[EnergyId.Health].Value.CurrentValue);

        s.Hit.Set(HitId.Default, 1, 0, 0, default, 2);
        Assert.Equal(HitId.Default, s.Hit.Id);
        Assert.Equal(1, s.Hit.Value);
        Assert.Equal(0, s.Hit.RootId);
        Assert.Equal(2, s.Hit.Token);
        Assert.Equal(default, s.Hit.Cycles);
        Assert.Equal(8, s.Energies.All[EnergyId.Health].Value.CurrentValue);
    }

    [Fact]
    public void Update()
    {
        var s = new Services();
        s.Hit.Set(HitId.Default, 1, 0, 0, default, 1);

        float interval = PlayConst.HitSeconds / 2;

        {
            s.Hit.Update(interval);

            Assert.Equal(HitId.Default, s.Hit.Id);
            Assert.Equal(1, s.Hit.Value);
            Assert.Equal(0, s.Hit.RootId);
            Assert.Equal(1, s.Hit.Token);
            Assert.Equal(new(0, 0.5f), s.Hit.Cycles);
            Assert.Equal(9, s.Energies.All[EnergyId.Health].Value.CurrentValue);
        }

        {
            s.Hit.Update(interval);

            Assert.Equal(HitId.Default, s.Hit.Id);
            Assert.Equal(1, s.Hit.Value);
            Assert.Equal(0, s.Hit.RootId);
            Assert.Equal(1, s.Hit.Token);
            Assert.Equal(new(0.5f, 1), s.Hit.Cycles);
            Assert.Equal(9, s.Energies.All[EnergyId.Health].Value.CurrentValue);
        }

        {
            s.Hit.Update(interval);

            Assert.Null(s.Hit.Id);
            Assert.Equal(0, s.Hit.Value);
            Assert.Equal(0, s.Hit.RootId);
            Assert.Equal(0, s.Hit.Token);
            Assert.Equal(default, s.Hit.Cycles);
            Assert.Equal(9, s.Energies.All[EnergyId.Health].Value.CurrentValue);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Update_ZeroOrNegativeDeltaSeconds_Throws(float deltaSeconds)
    {
        var s = new Services();

        Assert.Throws<ArgumentOutOfRangeException>(() => s.Hit.Update(deltaSeconds));
    }

    [Fact]
    public void Reset()
    {
        var s = new Services();
        s.Hit.Set(HitId.Default, 1, 0, 0, default, 1);
        s.Hit.Update(0.5f);

        s.Hit.Reset();

        Assert.Null(s.Hit.Id);
        Assert.Equal(0, s.Hit.Value);
        Assert.Equal(0, s.Hit.RootId);
        Assert.Equal(0, s.Hit.Token);
        Assert.Equal(default, s.Hit.Cycles);
    }

    private sealed class Services
    {
        public Services()
        {
            var energyInfos = new CharacterEnergyInfoGroup(new CharacterEnergyInfo[]
            {
                new(new(CharacterId.Player1, EnergyId.Health), 10)
            });

            Energies = new ObjectEnergyGroup(energyInfos);
            Energies.Set(CharacterId.Player1);
            Energies.SetValueRate();

            var clock = new FakeClock();
            var time = new Time(clock);
            Hit = new ObjectHit(Energies, time);
        }

        public ObjectHit Hit { get; }

        public ObjectEnergyGroup Energies { get; }
    }
}
