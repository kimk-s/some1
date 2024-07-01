using Some1.Play.Info;

namespace Some1.Play.Core.Internal;

public class ObjectEnergyTests
{
    [Theory]
    [InlineData(EnergyId.Health)]
    [InlineData(EnergyId.Mana)]
    public void Ctor(EnergyId id)
    {
        var s = new Services(id);

        Assert.Equal(id, s.Energy.Id);
        Assert.Equal(0, s.Energy.MaxValue.CurrentValue);
        Assert.Equal(0, s.Energy.Value.CurrentValue);
        Assert.False(s.Energy.IsFilledUp());
    }

    [Fact]
    public void SetMaxValue()
    {
        var s = new Services();

        {
            {
                s.Energy.Set(CharacterId.Player1);
                s.Energy.SetStatValue(0);

                Assert.Equal(10, s.Energy.MaxValue.CurrentValue);
                Assert.Equal(0, s.Energy.Value.CurrentValue);
                Assert.False(s.Energy.IsFilledUp());
            }

            {
                s.Energy.Set(CharacterId.Player2);
                s.Energy.SetStatValue(0);

                Assert.Equal(5, s.Energy.MaxValue.CurrentValue);
                Assert.Equal(0, s.Energy.Value.CurrentValue);
                Assert.False(s.Energy.IsFilledUp());
            }

            {
                s.Energy.Set(CharacterId.Player3);
                s.Energy.SetStatValue(0);

                Assert.Equal(0, s.Energy.MaxValue.CurrentValue);
                Assert.Equal(0, s.Energy.Value.CurrentValue);
                Assert.False(s.Energy.IsFilledUp());
            }
        }

        {
            {
                s.Energy.Set(CharacterId.Player1);
                s.Energy.SetStatValue(1);

                Assert.Equal(11, s.Energy.MaxValue.CurrentValue);
                Assert.Equal(0, s.Energy.Value.CurrentValue);
                Assert.False(s.Energy.IsFilledUp());
            }

            {
                s.Energy.Set(CharacterId.Player2);
                s.Energy.SetStatValue(1);

                Assert.Equal(5, s.Energy.MaxValue.CurrentValue);
                Assert.Equal(0, s.Energy.Value.CurrentValue);
                Assert.False(s.Energy.IsFilledUp());
            }

            {
                s.Energy.Set(CharacterId.Player3);
                s.Energy.SetStatValue(1);

                Assert.Equal(0, s.Energy.MaxValue.CurrentValue);
                Assert.Equal(0, s.Energy.Value.CurrentValue);
                Assert.False(s.Energy.IsFilledUp());
            }
        }

        {
            {
                s.Energy.Set(CharacterId.Player1);
                s.Energy.SetStatValue(2);

                Assert.Equal(12, s.Energy.MaxValue.CurrentValue);
                Assert.Equal(0, s.Energy.Value.CurrentValue);
                Assert.False(s.Energy.IsFilledUp());
            }

            {
                s.Energy.Set(CharacterId.Player2);
                s.Energy.SetStatValue(2);

                Assert.Equal(6, s.Energy.MaxValue.CurrentValue);
                Assert.Equal(0, s.Energy.Value.CurrentValue);
                Assert.False(s.Energy.IsFilledUp());
            }

            {
                s.Energy.Set(CharacterId.Player3);
                s.Energy.SetStatValue(2);

                Assert.Equal(0, s.Energy.MaxValue.CurrentValue);
                Assert.Equal(0, s.Energy.Value.CurrentValue);
                Assert.False(s.Energy.IsFilledUp());
            }
        }
    }

    [Fact]
    public void SetMaxValue_WhenHasValue_ThenSetValueByRatio()
    {
        var s = new Services();
        s.Energy.Set(CharacterId.Player1);
        s.Energy.SetStatValue(0);
        s.Energy.SetValue(5);

        {
            s.Energy.Set(CharacterId.Player2);

            Assert.Equal(5, s.Energy.MaxValue.CurrentValue);
            Assert.Equal(3, s.Energy.Value.CurrentValue);
            Assert.False(s.Energy.IsFilledUp());
        }

        {
            s.Energy.Set(CharacterId.Player1);

            Assert.Equal(10, s.Energy.MaxValue.CurrentValue);
            Assert.Equal(6, s.Energy.Value.CurrentValue);
            Assert.False(s.Energy.IsFilledUp());
        }
    }

    [Fact]
    public void SetValue()
    {
        var s = new Services();
        s.Energy.Set(CharacterId.Player1);

        {
            s.Energy.SetValue(1);

            Assert.Equal(10, s.Energy.MaxValue.CurrentValue);
            Assert.Equal(1, s.Energy.Value.CurrentValue);
            Assert.False(s.Energy.IsFilledUp());
        }

        {
            s.Energy.SetValue(10);

            Assert.Equal(10, s.Energy.MaxValue.CurrentValue);
            Assert.Equal(10, s.Energy.Value.CurrentValue);
            Assert.True(s.Energy.IsFilledUp());
        }
    }

    [Fact]
    public void SetValue_OutValue()
    {
        var s = new Services();
        s.Energy.Set(CharacterId.Player1);

        {
            s.Energy.SetValue(11);

            Assert.Equal(10, s.Energy.MaxValue.CurrentValue);
            Assert.Equal(10, s.Energy.Value.CurrentValue);
            Assert.True(s.Energy.IsFilledUp());
        }

        {
            s.Energy.SetValue(-1);

            Assert.Equal(10, s.Energy.MaxValue.CurrentValue);
            Assert.Equal(0, s.Energy.Value.CurrentValue);
            Assert.False(s.Energy.IsFilledUp());
        }
    }

    [Fact]
    public void Reset()
    {
        var s = new Services();
        s.Energy.Set(CharacterId.Player1);
        s.Energy.SetStatValue(1);

        s.Energy.Reset();

        Assert.Equal(EnergyId.Health, s.Energy.Id);
        Assert.Equal(0, s.Energy.MaxValue.CurrentValue);
        Assert.Equal(0, s.Energy.Value.CurrentValue);
        Assert.False(s.Energy.IsFilledUp());
    }

    [Fact]
    public void FillUp()
    {
        var s = new Services();
        s.Energy.Set(CharacterId.Player1);

        s.Energy.SetValueRate();

        Assert.Equal(10, s.Energy.MaxValue.CurrentValue);
        Assert.Equal(10, s.Energy.Value.CurrentValue);
        Assert.True(s.Energy.IsFilledUp());
    }

    [Fact]
    public void FillUp_WhenMaxValueIsZero()
    {
        var s = new Services();

        s.Energy.SetValueRate();

        Assert.Equal(0, s.Energy.MaxValue.CurrentValue);
        Assert.Equal(0, s.Energy.Value.CurrentValue);
        Assert.False(s.Energy.IsFilledUp());
    }

    [Fact]
    public void Clear()
    {
        var s = new Services();
        s.Energy.Set(CharacterId.Player1);
        s.Energy.SetValueRate();

        s.Energy.Clear();

        Assert.Equal(10, s.Energy.MaxValue.CurrentValue);
        Assert.Equal(0, s.Energy.Value.CurrentValue);
        Assert.True(s.Energy.IsCleared());
    }

    [Fact]
    public void CanConsume()
    {
        var s = new Services();
        s.Energy.Set(CharacterId.Player1);
        s.Energy.SetValueRate();
        bool result;

        result = s.Energy.CanConsume(1);
        Assert.True(result);
        Assert.Equal(10, s.Energy.MaxValue.CurrentValue);
        Assert.Equal(10, s.Energy.Value.CurrentValue);

        result = s.Energy.CanConsume(10);
        Assert.True(result);
        Assert.Equal(10, s.Energy.MaxValue.CurrentValue);
        Assert.Equal(10, s.Energy.Value.CurrentValue);

        result = s.Energy.CanConsume(11);
        Assert.False(result);
        Assert.Equal(10, s.Energy.MaxValue.CurrentValue);
        Assert.Equal(10, s.Energy.Value.CurrentValue);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CanConsume_ZeroOrNegativeCost_Throws(int cost)
    {
        var s = new Services();

        Assert.Throws<ArgumentOutOfRangeException>(() => s.Energy.CanConsume(cost));
    }

    [Fact]
    public void Consume()
    {
        var s = new Services();
        s.Energy.Set(CharacterId.Player1);
        s.Energy.SetValueRate();

        {
            s.Energy.Consume(1);
            Assert.Equal(10, s.Energy.MaxValue.CurrentValue);
            Assert.Equal(9, s.Energy.Value.CurrentValue);
            Assert.False(s.Energy.IsFilledUp());
        }

        {
            s.Energy.Consume(2);
            Assert.Equal(10, s.Energy.MaxValue.CurrentValue);
            Assert.Equal(7, s.Energy.Value.CurrentValue);
            Assert.False(s.Energy.IsFilledUp());
        }
    }

    [Fact]
    public void Consume_OverCost_Throws()
    {
        var s = new Services();
        s.Energy.Set(CharacterId.Player1);
        s.Energy.SetValueRate();

        Assert.Throws<InvalidOperationException>(() => s.Energy.Consume(11));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Consume_InvalidCost_Throws(int cost)
    {
        var s = new Services();

        Assert.Throws<ArgumentOutOfRangeException>(() => s.Energy.Consume(cost));
    }

    private sealed class Services
    {
        public Services() : this(EnergyId.Health)
        {
        }

        public Services(EnergyId id)
        {
            var infos = new CharacterEnergyInfoGroup(new CharacterEnergyInfo[]
            {
                new(new(CharacterId.Player1, EnergyId.Health), 10),
                new(new(CharacterId.Player2, EnergyId.Health), 5),
                new(new(CharacterId.Player3, EnergyId.Health), 0),
            });

            Energy = new(id, infos);
        }

        public ObjectEnergy Energy { get; }
    }
}
