using Some1.Play.Core.TestHelpers;
using ParallelOptions = Some1.Play.Core.Paralleling.ParallelOptions;

namespace Some1.Play.Core.Internal;

public class LeaderGroupTests
{
    [Fact]
    public void Ctor()
    {
        var s = new Services();

        Assert.Equal(s.All, s.Leaders.All);
    }

    [Fact]
    public void Lead1()
    {
        var s = new Services();
        s.EnableFirstLeader();

        s.Leaders.Lead1();

        Assert.Equal(LeaderStatus.Lead1Completed, s.Leaders.All[0].Status);
        Assert.Equal(LeaderStatus.None, s.Leaders.All[1].Status);
    }

    [Fact]
    public void Lead2()
    {
        var s = new Services();
        s.EnableFirstLeader();
        s.Leaders.Lead1();

        s.Leaders.Lead2();

        Assert.Equal(LeaderStatus.Lead2Completed, s.Leaders.All[0].Status);
        Assert.Equal(LeaderStatus.None, s.Leaders.All[1].Status);
    }

    [Fact]
    public void Lead3()
    {
        var s = new Services();
        s.EnableFirstLeader();
        s.Leaders.Lead1();
        s.Leaders.Lead2();

        s.Leaders.Lead3();

        Assert.Equal(LeaderStatus.Lead3Completed, s.Leaders.All[0].Status);
        Assert.Equal(LeaderStatus.None, s.Leaders.All[1].Status);
    }

    private sealed class Services
    {
        private readonly ObjectFactoryServices _ofs = new();

        public Services()
        {
            var objects = new Object[]
            {
                _ofs.ObjectFactory.CreateRoot(true),
                _ofs.ObjectFactory.CreateRoot(true),
            };

            All = new Leader[]
            {
                new Leader(objects[0], _ofs.Space),
                new Leader(objects[1], _ofs.Space)
            };

            Leaders = new LeaderGroup(All, ParallelOptions);

            _ofs.Time.Update(0.5f);

            objects[0].SetCharacter(Info.CharacterId.Player1);
            objects[1].SetCharacter(Info.CharacterId.Player1);
        }

        public LeaderGroup Leaders { get; }

        public ParallelOptions ParallelOptions => _ofs.ParallelOptions;

        public IReadOnlyList<Leader> All { get; }

        public void EnableFirstLeader() => All[0].Enable();
    }
}
