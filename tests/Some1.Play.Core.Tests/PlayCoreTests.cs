using Microsoft.Extensions.Logging;
using Some1.Data.InMemory;
using Some1.Play.Core.Options;
using Some1.Play.Core.TestHelpers;
using Some1.Play.Data.InMemory;
using Some1.Play.Info;
using Some1.Play.Info.Alpha;

namespace Some1.Play.Core;

public class PlayCoreTests
{
    [Fact]
    public void Ctor()
    {
        // Arrange
        var s = new Services();

        // Assert
        Assert.NotNull(s.Core.Leaders);

        Assert.NotNull(s.Core.Space);
        Assert.Equal(s.InfoRepository.Value.Space.SpaceTiles, s.Core.Space.Area.Size);

        Assert.NotNull(s.Core.Time);
    }

    [Fact]
    public void Update()
    {
        var s = new Services();

        s.Core.Update(0.5f);
        Assert.Equal(1, s.Core.Time.FrameCount);
        Assert.Equal(0.5f, s.Core.Time.DeltaSeconds);
        Assert.Equal(0.5f, s.Core.Time.TotalSeconds);

        s.Core.Update(0.5f);
        Assert.Equal(2, s.Core.Time.FrameCount);
        Assert.Equal(0.5f, s.Core.Time.DeltaSeconds);
        Assert.Equal(1, s.Core.Time.TotalSeconds);
    }

    private sealed class Services
    {
        public Services()
        {
            Clock = new FakeClock();
            InfoRepository = new AlphaPlayInfoRepository(AlphaPlayInfoEnvironment.Development);
            InfoRepository.LoadAsync(default).Wait();

            var playRepository = new InMemoryPlayRepository(
                new InMemoryRepository(
                    Microsoft.Extensions.Options.Options.Create<InMemoryRepositoryOptions>(new()
                    {
                        Plays = new InMemoryRepositoryOptionsPlay[]
                        {
                            new()
                            {
                                Id = "ap-seoul-1",
                            },
                        }
                    })));

            var options = Microsoft.Extensions.Options.Options.Create(new PlayOptions
            {
                Id = "ap-seoul-1",
                Parallel = new()
                {
                    Count = 1
                },
                Players = new()
                {
                    Count = 1,
                    Busy = new(),
                }
            });

            Core = new(Clock, InfoRepository, playRepository, options, new LoggerFactory());
        }

        public PlayCore Core { get; }

        public MemoryPlayInfoRepository InfoRepository { get; }

        public FakeClock Clock { get; }
    }
}
