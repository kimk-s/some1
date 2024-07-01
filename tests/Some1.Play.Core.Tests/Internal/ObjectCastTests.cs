using Some1.Play.Core.Paralleling;
using Some1.Play.Core.TestHelpers;
using Some1.Play.Info;
using ParallelOptions = Some1.Play.Core.Paralleling.ParallelOptions;

namespace Some1.Play.Core.Internal;

public class ObjectCastTests
{
    [Fact]
    public void SetCharacterId()
    {
        var s = new Services();

        s.Cast.Set(CharacterId.Player1);

        Assert.Null(s.Cast.Next);
        Assert.Null(s.Cast.Current);
        Assert.Equal(default, s.Cast.Cycles.Value.CurrentValue);
    }

    [Fact]
    public void SetCharacterId_WhenNotStopped_ThenStop()
    {

    }

    [Fact]
    public void SetNext()
    {
        var s = new Services();
        var next = new Cast(CastId.Attack, 1, true, new(0, 0));

        s.Cast.Set(next);

        Assert.Equal(next, s.Cast.Next);
        Assert.Null(s.Cast.Current);
        Assert.Equal(default, s.Cast.Cycles.Value.CurrentValue);
    }

    [Fact]
    public void Update_WhenCharacterIdIsNull()
    {
        var s = new Services();
        s.Cast.Set(new Cast(CastId.Attack, 1, true, new(0, 0)));
        s.Cast.SetBattle(true);
        s.Cast.SetIdle(true);
        s.Cast.FillCharge();

        s.UpdateCast(0.5f);

        Assert.NotNull(s.Cast.Next);
        Assert.Null(s.Cast.Current);
        Assert.Equal(default, s.Cast.Cycles.Value.CurrentValue);
    }

    [Fact]
    public void Update_WhenRequestIsNull()
    {
        var s = new Services();
        s.Cast.Set(CharacterId.Player1);
        s.Cast.SetBattle(true);
        s.Cast.SetIdle(true);
        s.Cast.FillCharge();

        s.UpdateCast(0.5f);

        Assert.Null(s.Cast.Next);
        Assert.Null(s.Cast.Current);
        Assert.Equal(default, s.Cast.Cycles.Value.CurrentValue);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Update_ZeroOrNegativeDeltaSeconds_Throws(float deltaSeconds)
    {
        var s = new Services();

        Assert.Throws<ArgumentOutOfRangeException>(() => s.UpdateCast(deltaSeconds));
    }

    [Fact]
    public void Update_WhenCharacterActionNotInInfos()
    {
        var s = new Services(new CharacterCastInfo(new(CharacterId.Player1, CastId.Super), 1, 1, 1, true, new(true, 0, 0), default));
        s.Cast.Set(CharacterId.Player1);
        s.Cast.SetBattle(true);
        s.Cast.SetIdle(true);
        s.Cast.FillCharge();
        s.Cast.Set(new Cast(CastId.Attack, 1, true, new(0, 0)));

        s.UpdateCast(0.5f);

        Assert.NotNull(s.Cast.Next);
        Assert.Null(s.Cast.Current);
        Assert.Equal(default, s.Cast.Cycles.Value.CurrentValue);
    }

    [Theory]
    [InlineData(1f)]
    [InlineData(2f)]
    [InlineData(3f)]
    public void Update_WithSeconds(float seconds)
    {
        var s = Services.CreateWithSeconds(seconds);
        s.Cast.Set(CharacterId.Player1);
        s.Cast.SetBattle(true);
        s.Cast.SetIdle(true);
        s.Cast.FillCharge();
        s.Cast.Set(new Cast(CastId.Attack, 1, true, new(0, 0)));

        s.UpdateCast(0.5f);

        Assert.NotNull(s.Cast.Next);
        Assert.Equal(s.Cast.Next, s.Cast.Current);
        Assert.Equal(new(0, 0.5f / seconds), s.Cast.Cycles.Value.CurrentValue);
    }

    [Fact]
    public void Update_WhenIsOffAndEqualsToken()
    {
        var s = new Services();
        s.Cast.Set(CharacterId.Player1);
        s.Cast.SetBattle(true);
        s.Cast.SetIdle(true);
        s.Cast.FillCharge();
        s.Cast.Set(new Cast(CastId.Attack, 0, false, new(0, 0)));

        s.UpdateCast(0.5f);

        Assert.NotNull(s.Cast.Next);
        Assert.Null(s.Cast.Current);
        Assert.Equal(default, s.Cast.Cycles.Value.CurrentValue);
    }

    [Fact]
    public void Update_WhenNotUseOnAndIsOn()
    {
        var s = Services.CreateWithNotUseOn();
        s.Cast.Set(CharacterId.Player1);
        s.Cast.SetBattle(true);
        s.Cast.SetIdle(true);
        s.Cast.FillCharge();
        s.Cast.Set(new Cast(CastId.Attack, 1, true, new(0, 0)));

        s.UpdateCast(0.5f);

        Assert.NotNull(s.Cast.Next);
        Assert.Null(s.Cast.Current);
        Assert.Equal(default, s.Cast.Cycles.Value.CurrentValue);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(90)]
    public void Update_WhenNotUseOnAndIsOff(float rotation)
    {
        var s = Services.CreateWithNotUseOn();
        s.Cast.Set(CharacterId.Player1);
        s.Cast.SetBattle(true);
        s.Cast.SetIdle(true);
        s.Cast.FillCharge();
        s.Cast.Set(new Cast(CastId.Attack, 1, false, new(rotation, 0)));

        {
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Equal(s.Cast.Next, s.Cast.Current);
            Assert.Equal(new(0, 0.5f), s.Cast.Cycles.Value.CurrentValue);
        }

        {
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Equal(s.Cast.Next, s.Cast.Current);
            Assert.Equal(new(0.5f, 1f), s.Cast.Cycles.Value.CurrentValue);
        }

        {
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Null(s.Cast.Current);
            Assert.Equal(s.Cast.Next.Value.Token, s.Cast.Token);
            Assert.Equal(default, s.Cast.Cycles.Value.CurrentValue);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(90)]
    public void Update_WhenNotUseOnAndIsOffDouble(float rotation)
    {
        var s = Services.CreateWithNotUseOn();
        s.Cast.Set(CharacterId.Player1);
        s.Cast.SetBattle(true);
        s.Cast.SetIdle(true);
        s.Cast.FillCharge();
        s.Cast.Set(new Cast(CastId.Attack, 1, false, new(rotation, 0)));

        {
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Equal(s.Cast.Next, s.Cast.Current);
            Assert.Equal(new(0, 0.5f), s.Cast.Cycles.Value.CurrentValue);
        }

        {
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Equal(s.Cast.Next, s.Cast.Current);
            Assert.Equal(new(0.5f, 1f), s.Cast.Cycles.Value.CurrentValue);

        }
        {
            s.Cast.Set(new Cast(CastId.Attack, 2, false, new(rotation, 0)));
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Equal(s.Cast.Next, s.Cast.Current);
            Assert.Equal(new(1f, 1.5f), s.Cast.Cycles.Value.CurrentValue);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(90)]
    public void Update_WhenUseOnAndIsOn(float rotation)
    {
        var s = new Services();
        s.Cast.Set(CharacterId.Player1);
        s.Cast.SetBattle(true);
        s.Cast.SetIdle(true);
        s.Cast.FillCharge();
        s.Cast.Set(new Cast(CastId.Attack, 1, true, new(rotation, 0)));

        {
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Equal(s.Cast.Next, s.Cast.Current);
            Assert.Equal(new(0, 0.5f), s.Cast.Cycles.Value.CurrentValue);
        }

        {
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Equal(s.Cast.Next, s.Cast.Current);
            Assert.Equal(new(0.5f, 1f), s.Cast.Cycles.Value.CurrentValue);
        }

        {
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Equal(s.Cast.Next, s.Cast.Current);
            Assert.Equal(new(1f, 1.5f), s.Cast.Cycles.Value.CurrentValue);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(90)]
    public void Update_WhenUseOnAndIsOff(float rotation)
    {
        var s = new Services();
        s.Cast.Set(CharacterId.Player1);
        s.Cast.SetBattle(true);
        s.Cast.SetIdle(true);
        s.Cast.FillCharge();
        s.Cast.Set(new Cast(CastId.Attack, 1, false, new(rotation, 0)));

        {
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Equal(s.Cast.Next, s.Cast.Current);
            Assert.Equal(new(0, 0.5f), s.Cast.Cycles.Value.CurrentValue);
        }

        {
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Null(s.Cast.Current);
            Assert.Equal(s.Cast.Next.Value.Token, s.Cast.Token);
            Assert.Equal(default, s.Cast.Cycles.Value.CurrentValue);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(90)]
    public void Update_WhenUseOnWithEdit(float rotation)
    {
        var s = new Services();
        s.Cast.Set(CharacterId.Player1);
        s.Cast.SetBattle(true);
        s.Cast.SetIdle(true);
        s.Cast.FillCharge();
        s.Cast.Set(new Cast(CastId.Attack, 1, true, new(rotation, 0)));

        {
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Equal(s.Cast.Next, s.Cast.Current);
            Assert.Equal(new(0, 0.5f), s.Cast.Cycles.Value.CurrentValue);
        }

        {
            // Edit
            s.Cast.Set(new Cast(CastId.Attack, 2, true, new(rotation + 10, 0)));
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Equal(s.Cast.Next, s.Cast.Current);
            Assert.Equal(new(0.5f, 1f), s.Cast.Cycles.Value.CurrentValue);
        }

        {
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Equal(s.Cast.Next, s.Cast.Current);
            Assert.Equal(new(1f, 1.5f), s.Cast.Cycles.Value.CurrentValue);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(90)]
    public void Update_WhenUseOnWithCancel(float rotation)
    {
        var s = new Services();
        s.Cast.Set(CharacterId.Player1);
        s.Cast.SetBattle(true);
        s.Cast.SetIdle(true);
        s.Cast.FillCharge();
        s.Cast.Set(new Cast(CastId.Attack, 1, true, new(rotation, 0)));

        {
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Equal(s.Cast.Next, s.Cast.Current);
            Assert.Equal(new(0, 0.5f), s.Cast.Cycles.Value.CurrentValue);
        }

        {
            // Cancel
            s.Cast.Set(new Cast(CastId.Attack, 2, false, new(rotation, 0)));
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Equal(s.Cast.Next, s.Cast.Current);
            Assert.Equal(new(0.5f, 1f), s.Cast.Cycles.Value.CurrentValue);
        }

        {
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Null(s.Cast.Current);
            Assert.Equal(s.Cast.Next.Value.Token, s.Cast.Token);
            Assert.Equal(FloatWave.Zero, s.Cast.Cycles.Value.CurrentValue);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(90)]
    public void Update_WhenNotUseRotation(float rotation)
    {
        var s = Services.CreateWithNotUseRotation();
        s.Cast.Set(CharacterId.Player1);
        s.Cast.SetBattle(true);
        s.Cast.SetIdle(true);
        s.Cast.FillCharge();
        s.Cast.Set(new Cast(CastId.Attack, 1, false, new(rotation, 0)));

        s.UpdateCast(0.5f);

        Assert.NotNull(s.Cast.Next);
        Assert.NotNull(s.Cast.Current);
        Assert.Equal(s.Cast.Next.Value.Id, s.Cast.Current.Value.Id);
        Assert.Equal(s.Cast.Next.Value.IsOn, s.Cast.Current.Value.IsOn);
        Assert.Equal(s.Cast.Next.Value.Token, s.Cast.Current.Value.Token);
        Assert.Equal(new(0, s.Cast.Next.Value.Aim.Length), s.Cast.Current.Value.Aim);
        Assert.Equal(new(0, 0.5f), s.Cast.Cycles.Value.CurrentValue);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(11)]
    public void Update_WhenClampDistance(float distance)
    {
        var s = Services.CreateWithClampDistance(0, 10);
        s.Cast.Set(CharacterId.Player1);
        s.Cast.SetBattle(true);
        s.Cast.SetIdle(true);
        s.Cast.FillCharge();
        s.Cast.Set(new Cast(CastId.Attack, 1, false, new(0, distance)));

        s.UpdateCast(0.5f);

        Assert.NotNull(s.Cast.Next);
        Assert.NotNull(s.Cast.Current);
        Assert.Equal(s.Cast.Next.Value.Id, s.Cast.Current.Value.Id);
        Assert.Equal(s.Cast.Next.Value.IsOn, s.Cast.Current.Value.IsOn);
        Assert.Equal(s.Cast.Next.Value.Token, s.Cast.Current.Value.Token);
        Assert.Equal(s.Cast.Next.Value.Aim.Rotation, s.Cast.Current.Value.Aim.Rotation);
        Assert.Equal(Math.Clamp(s.Cast.Next.Value.Aim.Length, 0, 10), s.Cast.Current.Value.Aim.Length);
        Assert.Equal(new(0, 0.5f), s.Cast.Cycles.Value.CurrentValue);
    }

    [Fact]
    public void Update_WhenWithLoadAndIsOff()
    {
        var s = Services.CreateWithLoad(1, 1, false);
        s.Cast.Set(CharacterId.Player1);
        s.Cast.SetBattle(true);
        s.Cast.SetIdle(true);
        s.Cast.Set(new Cast(CastId.Attack, 1, false, new(0, 0)));

        {
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Null(s.Cast.Current);
            Assert.Equal(1, s.Cast.Token);
            Assert.Equal(FloatWave.Zero, s.Cast.Cycles.Value.CurrentValue);
            Assert.Equal(new(0, 0.5f), s.Cast.Items[CastId.Attack].LoadWave.CurrentValue);
            Assert.False(s.Cast.Items[CastId.Attack].AnyLoadCount);
        }

        {
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Null(s.Cast.Current);
            Assert.Equal(1, s.Cast.Token);
            Assert.Equal(FloatWave.Zero, s.Cast.Cycles.Value.CurrentValue);
            Assert.Equal(new(0.5f, 1), s.Cast.Items[CastId.Attack].LoadWave.CurrentValue);
            Assert.True(s.Cast.Items[CastId.Attack].AnyLoadCount);
        }
    }

    [Fact]
    public void Update_WhenWithLoadCostAndIsOn()
    {
        var s = Services.CreateWithLoad(1, 1, true);
        s.Cast.Set(CharacterId.Player1);
        s.Cast.SetBattle(true);
        s.Cast.SetIdle(true);
        s.Cast.Set(new Cast(CastId.Attack, 1, true, new(0, 0)));

        {
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Null(s.Cast.Current);
            Assert.Equal(1, s.Cast.Token);
            Assert.Equal(FloatWave.Zero, s.Cast.Cycles.Value.CurrentValue);
            Assert.Equal(new(0, 0.5f), s.Cast.Items[CastId.Attack].LoadWave.CurrentValue);
            Assert.False(s.Cast.Items[CastId.Attack].AnyLoadCount);
        }

        {
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Equal(s.Cast.Next, s.Cast.Current);
            Assert.Equal(new(0, 0.5f), s.Cast.Cycles.Value.CurrentValue);
            Assert.Equal(FloatWave.Zero, s.Cast.Items[CastId.Attack].LoadWave.CurrentValue);
            Assert.False(s.Cast.Items[CastId.Attack].AnyLoadCount);
        }

        {
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Equal(s.Cast.Next, s.Cast.Current);
            Assert.Equal(new(0.5f, 1f), s.Cast.Cycles.Value.CurrentValue);
            Assert.Equal(new(0, 0.5f), s.Cast.Items[CastId.Attack].LoadWave.CurrentValue);
            Assert.False(s.Cast.Items[CastId.Attack].AnyLoadCount);
        }

        {
            s.UpdateCast(0.5f);

            Assert.NotNull(s.Cast.Next);
            Assert.Equal(s.Cast.Next, s.Cast.Current);
            Assert.Equal(new(1, 1.5f), s.Cast.Cycles.Value.CurrentValue);
            Assert.Equal(FloatWave.Zero, s.Cast.Items[CastId.Attack].LoadWave.CurrentValue);
            Assert.False(s.Cast.Items[CastId.Attack].AnyLoadCount);
        }
    }

    private sealed class Services
    {
        public Services() : this(CreateCharacterCastInfo())
        {
        }

        public Services(CharacterCastInfo info) : this(new CharacterCastInfo[] { info })
        {
        }

        public Services(IEnumerable<CharacterCastInfo> infoEnumerable)
        {
            var infos = new CharacterCastInfoGroup(infoEnumerable);
            var statInfos = new CharacterCastStatInfoGroup(Enumerable.Empty<CharacterCastStatInfo>());
            var characterStatInfos = new CharacterStatInfoGroup(Enumerable.Empty<CharacterStatInfo>());
            var characterEnergyInfos = new CharacterEnergyInfoGroup(new CharacterEnergyInfo[]
            {
                new(new(CharacterId.Player1, EnergyId.Health), 20),
            });
            
            var time = new Time(new FakeClock());
            var spaceInfo = new SpaceInfo(
                new(20, 40),
                4,
                new(20, 20),
                20,
                2,
                2,
                4,
                8,
                4,
                8);
            var parallelOptions = new ParallelOptions { Count = 1 };
            var space = new Space(spaceInfo, parallelOptions, time);
            var properties = new ObjectProperties();
            var trait = new ObjectTrait();
            var transform = new ObjectTransform(new FakeObjectRegion(), properties, time);
            Energies = new ObjectEnergyGroup(characterEnergyInfos);
            var stats = new ObjectStatGroup(characterStatInfos, Energies);

            Cast = new ObjectCast(
                1,
                infos,
                statInfos,
                stats,
                transform,
                trait,
                properties,
                space,
                time);

            stats.Set(CharacterId.Player1);
            Energies.Set(CharacterId.Player1);

            ParallelToken = new(0);
        }

        public ObjectCast Cast { get; }

        public ObjectEnergyGroup Energies { get; }

        public ParallelToken ParallelToken { get; }

        public void UpdateCast(float deltaSeconds)
        {
            Cast.Update(deltaSeconds, ParallelToken);
        }

        public static Services CreateWithSeconds(float seconds)
        {
            return new(CreateCharacterCastInfo(seconds, 1, 1, true, new(true, 0, 0)));
        }

        public static Services CreateWithLoad(float loadTime, int loadCount, bool useOn)
        {
            return new(CreateCharacterCastInfo(1f, loadTime, loadCount, useOn, new(true, 0, 0)));
        }

        public static Services CreateWithNotUseOn()
        {
            return new(CreateCharacterCastInfo(1f, 1, 1, false, new(true, 0, 0)));
        }

        public static Services CreateWithNotUseRotation()
        {
            return new(CreateCharacterCastInfo(1f, 1, 1, true, new(false, 0, 0)));
        }

        public static Services CreateWithClampDistance(float min, float max)
        {
            return new(CreateCharacterCastInfo(1f, 1, 1, true, new(true, min, max)));
        }

        public static CharacterCastInfo CreateCharacterCastInfo()
        {
            return CreateCharacterCastInfo(1f, 1, 1, true, new(true, 0, 0));
        }

        public static CharacterCastInfo CreateCharacterCastInfo(float cycle, float loadTime, int loadCount, bool useOn, CharacterCastAimLimitInfo aimLimit)
        {
            return new(new(CharacterId.Player1, CastId.Attack), cycle, loadTime, loadCount, useOn, aimLimit, default);
        }
    }
}
