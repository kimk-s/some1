namespace Some1.Play.Core.Internal;

public class ObjectMoveTests
{
    //[Theory]
    //[InlineData(2)]
    //[InlineData(1)]
    //[InlineData(-1)]
    //[InlineData(-2)]
    //public void Update_WithAccel(int accel)
    //{
    //    var s = new Services();
    //    s.Cast.Set(CharacterId.Character1);
    //    s.Cast.Set(new Cast(CastId.Attack, true, 1, 0, 0));
    //    s.Stats.All[StatId.Accel].SetCastValue(accel);

    //    s.Cast.Update(0.5f);

    //    Assert.Equal(StatHelper.Calculate(0.5f, accel), s.Cast.Cycles.Value);
    //}

    //[Theory]
    //[InlineData(1, 0)]
    //[InlineData(2, 0)]
    //[InlineData(10, 1)]
    //public void Update_WhenHasAccelStat(int value, int power)
    //{
    //    var s = Services.CreateHasStat(StatId.Accel, value, power);
    //    s.Cast.Set(CharacterId.Character1);
    //    s.Cast.Set(new Cast(CastId.Attack, false, 1, 0, 0));

    //    {
    //        s.Cast.Update(0.5f);

    //        Assert.Equal(CharacterId.Character1, s.Cast.CharacterId);
    //        Assert.NotNull(s.Cast.Next);
    //        Assert.Equal(s.Cast.Next, s.Cast.Current);
    //        Assert.Equal(new(StatHelper.Calculate(0.5f, s.Stats.All[StatId.Accel].Value), true), s.Cast.Cycles);
    //        Assert.NotNull(s.Cast.Info);
    //        Assert.NotNull(s.Cast.StatInfos);
    //        Assert.Equal(StatHelper.Calculate(value, power), s.Stats.All[StatId.Accel].Value);
    //    }

    //    {
    //        s.Cast.Update(0.5f);

    //        Assert.Equal(CharacterId.Character1, s.Cast.CharacterId);
    //        Assert.NotNull(s.Cast.Next);
    //        Assert.Null(s.Cast.Current);
    //        Assert.Equal(1, s.Cast.Token);
    //        Assert.Equal(default, s.Cast.Cycles);
    //        Assert.Null(s.Cast.Info);
    //        Assert.Null(s.Cast.StatInfos);
    //        Assert.Equal(0, s.Stats.All[StatId.Accel].Value);
    //    }
    //}
}
