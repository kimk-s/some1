using Some1.Play.Core.TestHelpers;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal;

public class ObjectHitGroupTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void Ctor(int count)
    {
        var s = new Services(count);

        Assert.Equal(
            Enumerable.Range(0, count).Select(_ => ((HitId?)null, 0, 0, 0, new FloatWave())),
            s.Hits.All.Select(x => (x.Id, x.Value, x.RootId, x.Token, x.Cycles)));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Ctor_ZeroOrNegativeCount_Throws(int count)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Services(count));
    }

    [Fact]
    public void Add()
    {
        var s = new Services(2);

        {
            s.Hits.Add(HitId.Default, 1, 0, 0, default);

            Assert.Equal(
                new[]
                {
                    ((HitId?)HitId.Default, 1, 0, 1),
                    (null, 0, 0, 0),
                },
                s.Hits.All.Select(x => (x.Id, x.Value, x.RootId, x.Token)));
        }

        {
            s.Hits.Add(HitId.Default, 2, 0, 0, default);

            Assert.Equal(
                new[]
                {
                    ((HitId?)HitId.Default, 1, 0, 1),
                    ((HitId?)HitId.Default, 2, 0, 2),
                },
                s.Hits.All.Select(x => (x.Id, x.Value, x.RootId, x.Token)));
        }

        {
            s.Hits.Add(HitId.Default, 3, 0, 0, default);

            Assert.Equal(
                new[]
                {
                    ((HitId?)HitId.Default, 3, 0, 3),
                    ((HitId?)HitId.Default, 2, 0, 2),
                },
                s.Hits.All.Select(x => (x.Id, x.Value, x.RootId, x.Token)));
        }
    }

    [Fact]
    public void Reset()
    {
        var s = new Services(2);
        s.Hits.Add(HitId.Default, 1, 0, 0, default);
        s.Hits.Add(HitId.Default, 2, 0, 0, default);

        s.Hits.Reset();

        Assert.Equal(
            Enumerable.Range(0, 2).Select(_ => ((HitId?)null, 0, 0, 0, new FloatWave())),
            s.Hits.All.Select(x => (x.Id, x.Value, x.RootId, x.Token, x.Cycles)));
    }

    [Fact]
    public void Update()
    {
        var s = new Services(2);
        s.Hits.Add(HitId.Default, 1, 0, 0, default);
        s.Hits.Add(HitId.Default, 2, 0, 0, default);

        s.Hits.Update(0.5f);

        Assert.All(s.Hits.All, x => Assert.Equal(new(0, 0.5f / PlayConst.HitSeconds), x.Cycles));
    }

    private sealed class Services
    {
        public Services(int count)
        {
            var energyInfos = new CharacterEnergyInfoGroup(new CharacterEnergyInfo[]
            {
                new(new(CharacterId.Player1, EnergyId.Health), 10)
            });

            var clock = new FakeClock();
            var time = new Time(clock);
            var energies = new ObjectEnergyGroup(energyInfos);
            Hits = new ObjectHitGroup(count, energies, time)
            {
                Enabled = true
            };
        }

        public ObjectHitGroup Hits { get; }
    }
}
