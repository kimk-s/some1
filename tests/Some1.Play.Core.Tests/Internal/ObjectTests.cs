using System.Drawing;
using System.Numerics;
using Some1.Play.Core.Paralleling;
using Some1.Play.Core.TestHelpers;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal;

public class ObjectTests
{
    [Fact]
    public void Ctor()
    {
        var s = new Services();

        Assert.NotEqual(0, s.Object.Id);
        Assert.False(s.Object.IsResetReserved);
        Assert.Null(s.Object.Character.Id.CurrentValue);
        Assert.False(s.Object.Alive.Alive.CurrentValue);
        Assert.Equal(FloatWave.Zero, s.Object.Alive.Cycles.Value.CurrentValue);
        Assert.Equal(Enum.GetValues<CastId>(), s.Object.Cast.Items.Values.Select(x => x.Id));
        Assert.Equal(Enumerable.Range(0, PlayConst.BuffCount).Select(_ => (BuffId?)null), s.Object.Buffs.Select(x => x.Id));
        Assert.Equal(Enumerable.Range(0, PlayConst.SpecialtyCount).Select(_ => (SpecialtyId?)null), s.Object.Specialties.Select(x => x.Id.CurrentValue));
        Assert.Equal(Enumerable.Range(0, PlayConst.TakeStuffCount).Select(_ => (Stuff?)null), s.Object.TakeStuffs.All.Select(x => x.Stuff));
        Assert.Equal(Enumerable.Range(0, PlayConst.HitCount).Select(_ => (HitId?)null), s.Object.Hits.Select(x => x.Id));
        Assert.Equal(Enum.GetValues<StatId>(), s.Object.Stats.Values.Select(x => x.Id));
        Assert.Equal(Enum.GetValues<EnergyId>(), s.Object.Energies.Values.Select(x => x.Id));
        Assert.False(s.Object.Transfer.IsMoveBlocked);
        Assert.Equal(Vector2.Zero, s.Object.Transfer.MoveDelta);
        Assert.Equal(Vector2Wave.Zero, s.Object.Transform.Position.CurrentValue);
        Assert.Equal(0, s.Object.Transform.Rotation.CurrentValue);
        Assert.Null(s.Object.Skin.Id);
        Assert.False(s.Object.Skin.Value);
        Assert.Null(s.Object.Emoji.Emoji);
        Assert.Equal(default, s.Object.Emoji.Cycles);
        Assert.Equal(default, s.Object.Properties.Area);
        Assert.False(s.Object.Control.IsTaken);
    }

    [Fact]
    public void SetCharacterId()
    {
        var s = new Services();

        s.Object.SetCharacter(CharacterId.Player1);

        Assert.Equal(CharacterId.Player1, s.Object.Character.Id.CurrentValue);
        Assert.All(s.Object.Stats.Values.Where(x => x.Id != StatId.StunCast && x.Id != StatId.StunWalk).Select(x => x.Value), x => Assert.Equal(1, x));
        Assert.All(s.Object.Energies.Values.Select(x => (x.Value.CurrentValue, x.MaxValue.CurrentValue)), x => Assert.Equal((0, 11), x));
    }

    [Fact]
    public void SetCharacterId_StopAction()
    {
    }

    [Fact]
    public void SetBattleMode()
    {
        var s = new Services();
        s.Object.SetCharacter(CharacterId.Player1);

        s.Object.SetBattle(true);
        s.Object.SetBattle(false);
    }

    [Fact]
    public void SetCastNext()
    {
        var s = new Services();
        s.Object.SetCharacter(CharacterId.Player1);
        s.Object.SetBattle(true);
        s.Object.SetSkin(SkinId.Skin0);
        var next = new Cast(CastId.Attack, 1, true, new(0, 0));

        s.Object.SetCast(next);

        Assert.Equal(next, s.Object.Cast.Next);
    }

    [Fact]
    public void SetCastNext_Ignore()
    {
        var s = new Services();
        s.Object.SetCharacter(CharacterId.Player1);
        var next = new Cast(CastId.Attack, 1, true, new(0, 0));

        s.Object.SetCast(next);

        Assert.Null(s.Object.Cast.Next);
    }

    [Fact]
    public void SetCastNext_WhenCharacterIdIsNull_False()
    {
        var s = new Services();

        Assert.False(s.Object.SetCast(new Cast()));
    }

    [Fact]
    public void SetWalk()
    {
        var s = new Services();
        s.Object.SetCharacter(CharacterId.Player1);
        s.Object.SetBattle(true);
        s.Object.SetSkin(SkinId.Skin0);
        var walk = new Walk(true, 0);

        s.Object.SetWalk(walk);

        Assert.Equal(walk, s.Object.Walk.Walk);
    }

    [Fact]
    public void SetWalk_Ignore()
    {
        var s = new Services();
        s.Object.SetCharacter(CharacterId.Player1);
        var walk = new Walk(true, 0);

        s.Object.SetWalk(walk);

        Assert.Equal(default, s.Object.Walk.Walk);
    }

    [Fact]
    public void SetWalk_WhenCharacterIdIsNull_False()
    {
        var s = new Services();

        Assert.False(s.Object.SetWalk(new Walk()));
    }

    [Fact]
    public void SetPosition()
    {
        var s = new Services();
        s.Object.SetCharacter(CharacterId.Player1);

        s.Object.SetTransfer(new(1, 1), null);

        Assert.True(s.Object.Transfer.IsMoveBlocked);
        Assert.Equal(Vector2.Zero, s.Object.Transfer.MoveDelta);
        Assert.Equal(Area.Rectangle(new Vector2(1, 1), new SizeF(1, 1)), s.Object.Properties.Area);
    }

    [Fact]
    public void SetPosition_WhenCharacterIdIsNull_Throws()
    {
        var s = new Services();

        Assert.Throws<InvalidOperationException>(() => s.Object.SetTransfer(new(1, 1), null));
    }

    [Fact]
    public void SetRotation()
    {
        var s = new Services();
        s.Object.SetCharacter(CharacterId.Player1);

        s.Object.SetTransformRotation(1);

        Assert.Equal(1, s.Object.Transform.Rotation.CurrentValue);
    }

    [Fact]
    public void SetRotation_WhenCharacterIdIsNull_Throws()
    {
        var s = new Services();

        Assert.Throws<InvalidOperationException>(() => s.Object.SetTransformRotation(1));
    }

    [Fact]
    public void SetSkinId()
    {
        var s = new Services();
        s.Object.SetCharacter(CharacterId.Player1);

        s.Object.SetSkin(SkinId.Skin0);

        Assert.Equal(SkinId.Skin0, s.Object.Skin.Id);
        Assert.True(s.Object.Skin.Value);
    }

    [Fact]
    public void SetSkinId_NotInInfosButDefaultExists()
    {
        var s = new Services();
        s.Object.SetCharacter(CharacterId.Player1);

        s.Object.SetSkin(SkinId.Skin1);

        Assert.Equal(SkinId.Skin1, s.Object.Skin.Id);
        Assert.True(s.Object.Skin.Value);
    }

    [Fact]
    public void SetSkinId_WhenCharacterIdIsNull_Throws()
    {
        var s = new Services();

        Assert.Throws<InvalidOperationException>(() => s.Object.SetSkin(SkinId.Skin0));
    }

    [Fact]
    public void SetTeam()
    {
        var s = new Services();
        s.Object.SetCharacter(CharacterId.Player1);

        s.Object.SetTeamProperty(Team.Player);

        Assert.Equal(Team.Player, s.Object.Properties.Team.CurrentValue);
    }

    [Fact]
    public void SetTeam_WhenCharacterIdIsNull_Throws()
    {
        var s = new Services();

        Assert.Throws<InvalidOperationException>(() => s.Object.SetTeamProperty(Team.Player));
    }

    [Fact]
    public void TryUpdate_True()
    {
        var s = new Services();
        s.Object.SetCharacter(CharacterId.Player1);
        s.Object.SetTransfer(new(0.5f, 0.5f), null);
        s.Time.Update(0.5f);

        bool result = s.Object.Update(true, s.ParallelToken);

        Assert.True(result);
        Assert.True(s.Object.Control.IsTaken);
    }

    [Fact]
    public void TryUpdate_DifferentLeader_False()
    {
        var s = new Services();
        s.Object.SetCharacter(CharacterId.Player1);
        s.Object.SetTransfer(new(0.5f, 0.5f), null);
        s.Time.Update(0.5f);

        bool result = s.Object.Update(false, s.ParallelToken);

        Assert.False(result);
        Assert.False(s.Object.Control.IsTaken);
    }

    [Fact]
    public void TryUpdate_Duplicated_False()
    {
        var s = new Services();
        s.Object.SetCharacter(CharacterId.Player1);
        s.Object.SetTransfer(new(0.5f, 0.5f), null);
        s.Time.Update(0.5f);

        s.Object.Update(true, s.ParallelToken);
        bool result = s.Object.Update(true, s.ParallelToken);

        Assert.False(result);
        Assert.True(s.Object.Control.IsTaken);
    }

    [Fact]
    public void Reset()
    {
        var s = new Services();
        s.Object.SetCharacter(CharacterId.Player1);
        s.Object.SetBattle(true);
        s.Object.SetCast(new Cast());
        s.Object.SetTransfer(new(1, 1), null);
        s.Object.SetSkin(SkinId.Skin0);
        s.Object.SetTeamProperty(Team.Player);

        s.Object.Reset(null);

        Assert.NotEqual(0, s.Object.Id);
        Assert.False(s.Object.IsResetReserved);
        Assert.Null(s.Object.Character.Id.CurrentValue);
        Assert.False(s.Object.Alive.Alive.CurrentValue);
        Assert.Equal(FloatWave.Zero, s.Object.Alive.Cycles.Value.CurrentValue);
        Assert.Equal(Enum.GetValues<CastId>(), s.Object.Cast.Items.Values.Select(x => x.Id));
        Assert.Equal(Enumerable.Range(0, PlayConst.BuffCount).Select(_ => (BuffId?)null), s.Object.Buffs.Select(x => x.Id));
        Assert.Equal(Enumerable.Range(0, PlayConst.SpecialtyCount).Select(_ => (SpecialtyId?)null), s.Object.Specialties.Select(x => x.Id.CurrentValue));
        Assert.Equal(Enumerable.Range(0, PlayConst.TakeStuffCount).Select(_ => (Stuff?)null), s.Object.TakeStuffs.All.Select(x => x.Stuff));
        Assert.Equal(Enumerable.Range(0, PlayConst.HitCount).Select(_ => (HitId?)null), s.Object.Hits.Select(x => x.Id));
        Assert.Equal(Enum.GetValues<StatId>(), s.Object.Stats.Values.Select(x => x.Id));
        Assert.Equal(Enum.GetValues<EnergyId>(), s.Object.Energies.Values.Select(x => x.Id));
        Assert.False(s.Object.Transfer.IsMoveBlocked);
        Assert.Equal(Vector2.Zero, s.Object.Transfer.MoveDelta);
        Assert.Equal(Vector2Wave.Zero, s.Object.Transform.Position.CurrentValue);
        Assert.Equal(0, s.Object.Transform.Rotation.CurrentValue);
        Assert.Null(s.Object.Skin.Id);
        Assert.False(s.Object.Skin.Value);
        Assert.Null(s.Object.Emoji.Emoji);
        Assert.Equal(0, s.Object.Emoji.Cycles);
        Assert.Equal(Area.Empty, s.Object.Properties.Area);
        Assert.False(s.Object.Control.IsTaken);
    }

    private sealed class Services
    {
        private readonly ObjectFactoryServices _ofs = new();

        public Services()
        {
            Object = _ofs.ObjectFactory.CreateRoot(true);
        }

        public Object Object { get; }

        public Time Time => _ofs.Time;

        public ParallelToken ParallelToken { get; } = new(0);

        public void Update() => _ofs.Update(0.5f);
    }
}
