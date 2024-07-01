using Microsoft.Extensions.Logging;
using Some1.Data.InMemory;
using Some1.Play.Core.Options;
using Some1.Play.Core.TestHelpers;
using Some1.Play.Data.InMemory;

namespace Some1.Play.Core.Internal;

public class PlayerGroupTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void Ctor(int count)
    {
        var s = new Services(count);

        Assert.Equal(count, s.Players.All.Count);
        Assert.Equal(0, s.Players.UidCount);
        Assert.Equal(count, s.Players.NonUidCount);
    }

    private sealed class Services
    {
        private readonly ObjectFactoryServices _ofs = new();

        public Services(int count)
        {
            var rankings = new RankingGroup(() => Players ?? throw new InvalidOperationException(), _ofs.Time);

            var options = new PlayerGroupOptions()
            {
                Count = count,
                Busy = new()
            };

            var playRepository = new InMemoryPlayRepository(
                new InMemoryRepository(
                    Microsoft.Extensions.Options.Options.Create<InMemoryRepositoryOptions>(new()
                    {
                        Plays = Array.Empty<InMemoryRepositoryOptionsPlay>()
                    })));

            Players = new(
                "",
                options,
                playRepository,
                _ofs.Info.Characters,
                _ofs.Info.CharacterSkins,
                _ofs.Info.Specialties,
                _ofs.ObjectFactory,
                rankings,
                _ofs.Regions,
                _ofs.Space,
                _ofs.Time,
                new LoggerFactory());
        }

        public PlayerGroup Players { get; }
    }
}
