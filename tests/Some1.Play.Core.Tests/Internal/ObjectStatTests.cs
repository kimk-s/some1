using Some1.Play.Info;

namespace Some1.Play.Core.Internal;

public class ObjectStatTests
{
    [Theory]
    [InlineData(StatId.Accel)]
    [InlineData(StatId.Offense)]
    public void Ctor(StatId id)
    {
        var s = new Services(id);

        Assert.Equal(id, s.Stat.Id);
        Assert.Equal(0, s.Stat.Value);
    }

    [Fact]
    public void SetValue()
    {
        var s = new Services(StatId.Health);

        {
            s.Stat.Set(CharacterId.Player1);
            s.Stat.SetCastValue(0);
            s.Stat.SetBuffValue(0, 0);

            Assert.Equal(1, s.Stat.Value);
            Assert.Equal(11, s.Energies.All[EnergyId.Health].MaxValue.CurrentValue);
            Assert.Equal(10, s.Energies.All[EnergyId.Mana].MaxValue.CurrentValue);
        }

        {
            s.Stat.Set(CharacterId.Player2);
            s.Stat.SetCastValue(1);
            s.Stat.SetBuffValue(0, 0);

            Assert.Equal(1, s.Stat.Value);
            Assert.Equal(11, s.Energies.All[EnergyId.Health].MaxValue.CurrentValue);
            Assert.Equal(10, s.Energies.All[EnergyId.Mana].MaxValue.CurrentValue);
        }

        {
            s.Stat.Set(CharacterId.Player2);
            s.Stat.SetCastValue(0);
            s.Stat.SetBuffValue(1, 0);

            Assert.Equal(1, s.Stat.Value);
            Assert.Equal(11, s.Energies.All[EnergyId.Health].MaxValue.CurrentValue);
            Assert.Equal(10, s.Energies.All[EnergyId.Mana].MaxValue.CurrentValue);
        }

        {
            s.Stat.Set(CharacterId.Player1);
            s.Stat.SetCastValue(1);
            s.Stat.SetBuffValue(1, 0);

            Assert.Equal(3, s.Stat.Value);
            Assert.Equal(13, s.Energies.All[EnergyId.Health].MaxValue.CurrentValue);
            Assert.Equal(10, s.Energies.All[EnergyId.Mana].MaxValue.CurrentValue);
        }
    }

    [Theory]
    [InlineData(PlayConst.MinStatValue - 1)]
    [InlineData(PlayConst.MaxStatValue + 1)]
    public void SetValue_OverValue(int value)
    {
        var s = new Services(value);

        s.Stat.Set(CharacterId.Player1);

        Assert.Equal(Math.Clamp(value, PlayConst.MinStatValue, PlayConst.MaxStatValue), s.Stat.Value);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(PlayConst.BuffCount)]
    public void SetBuffValue_OutIndex_Throws(int index)
    {
        var s = new Services();

        Assert.Throws<IndexOutOfRangeException>(() => s.Stat.SetBuffValue(0, index));
    }

    [Fact]
    public void Reset()
    {
        var s = new Services();
        s.Stat.Set(CharacterId.Player1);
        s.Stat.SetCastValue(1);
        s.Stat.SetBuffValue(1, 0);

        s.Stat.Reset();

        Assert.Equal(StatId.Accel, s.Stat.Id);
        Assert.Equal(0, s.Stat.Value);
        Assert.All(s.Energies.All.Values, x => Assert.Equal(10, x.MaxValue.CurrentValue));
    }

    private sealed class Services
    {
        public Services() : this(StatId.Accel, 1)
        {
        }

        public Services(StatId id) : this(id, 1)
        {
        }

        public Services(int value) : this(StatId.Accel, value)
        {
        }

        public Services(StatId id, int value)
        {
            var infos = new CharacterStatInfoGroup(new CharacterStatInfo[]
            {
                new(new(CharacterId.Player1, id), value),
            });
            var energyInfos = new CharacterEnergyInfoGroup(new CharacterEnergyInfo[]
            {
                new(new(CharacterId.Player1, EnergyId.Health), 10),
                new(new(CharacterId.Player1, EnergyId.Mana), 10),
            });
            Energies = new ObjectEnergyGroup(energyInfos);
            Energies.Set(CharacterId.Player1);

            Stat = new ObjectStat(id, infos, Energies);
        }

        public ObjectStat Stat { get; }

        public ObjectEnergyGroup Energies { get; }
    }
}
