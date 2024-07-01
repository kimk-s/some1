using Some1.Play.Core.Paralleling;
using Some1.Play.Core.TestHelpers;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal;

public class ObjectBuffGroupTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void Ctor(int count)
    {
        var s = new Services(false, count);

        Assert.Equal(
            Enumerable.Range(0, count).Select(_ => ((BuffId?)null, 0, 0, new FloatWave())),
            s.Buffs.All.Select(x => (x.Id, x.RootId, x.Token, x.Cycles.Value.CurrentValue)));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Ctor_ZeroOrNegativeCount_Throws(int count)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Services(false, count));
    }

    [Fact]
    public void Set()
    {
        var s = new Services(false, 2);

        {
            s.Buffs.Add(BuffId.Buff1, 1, default, default, 1, default, default);

            Assert.Equal(
                new[]
                {
                    ((BuffId?)BuffId.Buff1, 1, 1),
                    (null, 0, 0),
                },
                s.Buffs.All.Select(x => (x.Id, x.RootId, x.Token)));
        }

        {
            s.Buffs.Add(BuffId.Buff1, 1, default, default, 1, default, default);

            Assert.Equal(
                new[]
                {
                    ((BuffId?)BuffId.Buff1, 1, 1),
                    ((BuffId?)BuffId.Buff1, 1, 2),
                },
                s.Buffs.All.Select(x => (x.Id, x.RootId, x.Token)));
        }

        {
            s.Buffs.Add(BuffId.Buff1, 1, default, default, 1, default, default);

            Assert.Equal(
                new[]
                {
                    ((BuffId?)BuffId.Buff1, 1, 3),
                    ((BuffId?)BuffId.Buff1, 1, 2),
                },
                s.Buffs.All.Select(x => (x.Id, x.RootId, x.Token)));
        }
    }

    [Fact]
    public void Set_WhenIsUnique()
    {
        var s = new Services(true, 2);

        {
            s.Buffs.Add(BuffId.Buff1, 1, default, default, 1, default, default);

            Assert.Equal(
                new[]
                {
                    ((BuffId?)BuffId.Buff1, 1, 1),
                    (null, 0, 0),
                },
                s.Buffs.All.Select(x => (x.Id, x.RootId, x.Token)));
        }

        {
            s.Buffs.Add(BuffId.Buff1, 1, default, default, 1, default, default);

            Assert.Equal(
                new[]
                {
                    ((BuffId?)BuffId.Buff1, 1, 2),
                    (null, 0, 0),
                },
                s.Buffs.All.Select(x => (x.Id, x.RootId, x.Token)));
        }
    }

    [Fact]
    public void Reset()
    {
        var s = new Services(false, 2);
        s.Buffs.Add(BuffId.Buff1, 1, default, default, 1, default, default);
        s.Buffs.Add(BuffId.Buff1, 1, default, default, 1, default, default);

        s.Buffs.Reset();

        Assert.Equal(
            Enumerable.Range(0, 2).Select(_ => ((BuffId?)null, 0, 0, new FloatWave())),
            s.Buffs.All.Select(x => (x.Id, x.RootId, x.Token, x.Cycles.Value.CurrentValue)));
    }

    [Fact]
    public void Update()
    {
        var s = new Services(false, 2);
        s.Buffs.Add(BuffId.Buff1, 1, default, default, 1, default, default);
        s.Buffs.Add(BuffId.Buff1, 1, default, default, 1, default, default);

        s.Buffs.Update(0.5f, new(0));

        Assert.All(s.Buffs.All, x => Assert.Equal(new(0, 0.5f), x.Cycles.Value.CurrentValue));
    }

    private sealed class Services
    {
        public Services(bool isUnique, int count)
        {
            var infos = new BuffInfoGroup(new[] { new BuffInfo(BuffId.Buff1, 1f, isUnique) });
            var statInfos = new BuffStatInfoGroup(Enumerable.Empty<BuffStatInfo>());
            var characterStatInfos = new CharacterStatInfoGroup(Enumerable.Empty<CharacterStatInfo>());
            var characterEnergyInfos = new CharacterEnergyInfoGroup(Enumerable.Empty<CharacterEnergyInfo>());

            var clock = new FakeClock();
            var time = new Time(clock);
            var trait = new ObjectTrait();
            var energies = new ObjectEnergyGroup(characterEnergyInfos);
            var stats = new ObjectStatGroup(characterStatInfos, energies);
            var hits = new ObjectHitGroup(1, energies, time);

            Buffs = new ObjectBuffGroup(
                count,
                infos,
                statInfos,
                hits,
                stats,
                1,
                trait,
                time)
            {
                Enabled = true
            };
        }

        public ObjectBuffGroup Buffs { get; }
    }
}
