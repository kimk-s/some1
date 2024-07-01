using System.IO.Pipelines;
using Microsoft.Extensions.Logging;
using Some1.Net;
using Some1.Play.Core.TestHelpers;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal;

public class PlayerTests
{
    [Fact]
    public void Ctor()
    {
        var s = new Services();

        Assert.Null(s.Player.UserId);
        Assert.Equal(PipeStatus.None, s.Player.PipeState.Status);
        Assert.Equal(PlayerDataStatus.None, s.Player.DataStatus);
        Assert.Equal(
            s.Info.Characters.ById.Values.Where(x => x.IsPlayer).Select(x => x.Id),
            s.Player.Characters.All.Keys);
        Assert.Equal(s.Info.Specialties.ById.Keys, s.Player.Specialties.Keys);
        Assert.NotNull(s.Player.Leader);
        Assert.NotNull(s.Player.Object);
    }

    [Fact]
    public void Set()
    {
        var s = new Services();
        var userId = "a";

        s.Player.SetUidPipe(new(userId, CreateFakeDuplexPipe()));

        Assert.Equal(userId, s.Player.UserId);
        Assert.Equal(PipeStatus.Processing, s.Player.PipeState.Status);
        Assert.Equal(PlayerDataStatus.None, s.Player.DataStatus);
        Assert.Equal(PlayerId.Empty, s.Player.Title.PlayerId);
    }

    [Fact]
    public void Data()
    {
        var s = new Services();
        s.Player.SetUidPipe(new("a", CreateFakeDuplexPipe()));

        {
            var id = s.Player.BeginLoad();

            Assert.Equal("a", id);
            Assert.Equal(PlayerDataStatus.BeginLoad, s.Player.DataStatus);
        }

        {
            s.Player.EndLoad(
                new()
                {
                    Id = "a",
                    PlayId = "ap-seoul-1",
                    IsPlaying = true,
                    PackedData = null,
                });

            Assert.Equal("a", s.Player.UserId);
            Assert.Equal(PlayerDataStatus.EndLoad, s.Player.DataStatus);
            Assert.Equal(RegionId.Region1, s.Regions.Get(s.Player.Object.Transform.Position.CurrentValue.B)?.Id.Region);
        }

        {
            var playerData = s.Player.BeginSave();

            Assert.Equal(s.Player.UserId, playerData.Id);
            Assert.Equal("ap-seoul-1", playerData.PlayId);
            Assert.True(playerData.IsPlaying);
            Assert.False(playerData.Manager);
            Assert.Equal(PlayerDataStatus.BeginSave, s.Player.DataStatus);
        }

        {
            s.Player.EndSave(new("a", null));

            Assert.Equal(PlayerDataStatus.EndSave, s.Player.DataStatus);
        }
    }

    private sealed class Services
    {
        private readonly ObjectFactoryServices _ofs = new();

        public Services()
        {
            var rankings = new RankingGroup(() => null!, _ofs.Time);

            Player = new(
                "ap-seoul-1",
                _ofs.Info.Characters,
                _ofs.Info.CharacterSkins,
                _ofs.Info.Specialties,
                _ofs.ObjectFactory,
                rankings,
                _ofs.Regions,
                _ofs.Space,
                _ofs.Time,
                LoggerFactory.Create(x => x.ClearProviders()).CreateLogger<Player>());

            Regions = _ofs.Regions;
        }

        public Player Player { get; }

        public PlayInfo Info => _ofs.Info;

        public RegionGroup Regions { get; }
    }

    private static DuplexPipe CreateFakeDuplexPipe()
    {
        var pipe = new Pipe();
        return new DuplexPipe(pipe.Reader, pipe.Writer, () => { });
    }
}
