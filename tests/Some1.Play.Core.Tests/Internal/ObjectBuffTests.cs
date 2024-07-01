using Some1.Play.Core.Paralleling;
using Some1.Play.Core.TestHelpers;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal;

public class ObjectBuffTests
{
    [Fact]
    public void Ctor()
    {
        var s = new Services();

        Assert.Equal(0, s.Buff.Index);
        Assert.Null(s.Buff.Id);
        Assert.Equal(0, s.Buff.RootId);
        Assert.Equal(0, s.Buff.Token);
        Assert.Equal(FloatWave.Zero, s.Buff.Cycles.Value.CurrentValue);
    }

    [Fact]
    public void Set()
    {
        var s = new Services();

        s.Buff.Set(BuffId.Buff1, 1,  default, default, default, default, default, 1);

        Assert.Equal(BuffId.Buff1, s.Buff.Id);
        Assert.Equal(1, s.Buff.RootId);
        Assert.Equal(1, s.Buff.Token);
        Assert.Equal(FloatWave.Zero, s.Buff.Cycles.Value.CurrentValue);
    }

    [Theory]
    [InlineData(BuffId.Buff2)]
    public void Set_IdNotInInfos_Throws(BuffId id)
    {
        var s = new Services();

        Assert.ThrowsAny<Exception>(() => s.Buff.Set(id, 1, default, default, default, default, default, 0));
    }

    [Fact]
    public void Update()
    {
        var s = new Services();
        s.Buff.Set(BuffId.Buff1, 1, default, default, default, default, default, 1);

        {
            s.UpdateBuff(0.5f);

            Assert.Equal(BuffId.Buff1, s.Buff.Id);
            Assert.Equal(1, s.Buff.RootId);
            Assert.Equal(1, s.Buff.Token);
            Assert.Equal(new(0, 0.5f), s.Buff.Cycles.Value.CurrentValue);
        }

        {
            s.UpdateBuff(0.5f);

            Assert.Equal(BuffId.Buff1, s.Buff.Id);
            Assert.Equal(1, s.Buff.RootId);
            Assert.Equal(1, s.Buff.Token);
            Assert.Equal(new(0.5f, 1), s.Buff.Cycles.Value.CurrentValue);
        }

        {
            s.UpdateBuff(0.5f);

            Assert.Null(s.Buff.Id);
            Assert.Equal(0, s.Buff.RootId);
            Assert.Equal(0, s.Buff.Token);
            Assert.Equal(default, s.Buff.Cycles.Value.CurrentValue);
        }
    }

    [Theory]
    [InlineData(1f)]
    [InlineData(2f)]
    [InlineData(3f)]
    public void Update_WithSeconds(float seconds)
    {
        var s = Services.CreateWithSeconds(seconds);
        s.Buff.Set(BuffId.Buff1, 1, default, default, default, default, default, 1);

        s.UpdateBuff(0.5f);

        Assert.Equal(BuffId.Buff1, s.Buff.Id);
        Assert.Equal(1, s.Buff.RootId);
        Assert.Equal(1, s.Buff.Token);
        Assert.Equal(new(0, 0.5f / seconds), s.Buff.Cycles.Value.CurrentValue);
    }

    [Fact]
    public void Reset()
    {
        var s = new Services();
        s.Buff.Set(BuffId.Buff1, 1, default, default, default, default, default, 1);
        s.UpdateBuff(0.5f);

        s.Buff.Reset();

        Assert.Null(s.Buff.Id);
        Assert.Equal(0, s.Buff.RootId);
        Assert.Equal(0, s.Buff.Token);
        Assert.Equal(FloatWave.Zero, s.Buff.Cycles.Value.CurrentValue);
    }

    private sealed class Services
    {
        public Services() : this(new[] { CreateBuffInfo() })
        {
        }

        public Services(BuffInfo info) : this(new[] { info })
        {
        }

        public Services(IEnumerable<BuffInfo> infoEnumerable)
        {
            var infos = new BuffInfoGroup(infoEnumerable);
            var statInfos = new BuffStatInfoGroup(Enumerable.Empty<BuffStatInfo>());
            var characterStatInfos = new CharacterStatInfoGroup(Enumerable.Empty<CharacterStatInfo>());
            var characterEnergyInfos = new CharacterEnergyInfoGroup(Enumerable.Empty<CharacterEnergyInfo>());

            var clock = new FakeClock();
            var time = new Time(clock);
            var trait = new ObjectTrait();
            var energies = new ObjectEnergyGroup(characterEnergyInfos);
            var stats = new ObjectStatGroup(characterStatInfos, energies);
            var hits = new ObjectHitGroup(1, energies, time);

            var buffs = new ObjectBuffGroup(
                1,
                infos,
                statInfos,
                hits,
                stats,
                1,
                trait,
                time);
            Buff = (ObjectBuff)buffs.All[0];
        }

        public ObjectBuff Buff { get; }

        public void UpdateBuff(float deltaSeconds)
        {
            Buff.Update(deltaSeconds, new(0));
        }

        public static Services CreateWithSeconds(float seconds)
        {
            return new(CreateBuffInfo(seconds));
        }

        public static BuffInfo CreateBuffInfo()
        {
            return CreateBuffInfo(1f);
        }

        public static BuffInfo CreateBuffInfo(float seconds)
        {
            return new(BuffId.Buff1, seconds, false);
        }
    }
}
