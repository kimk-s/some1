using Some1.Play.Info.TestHelpers;

namespace Some1.Play.Info;

public class MemoryPlayInfoRepositoryTests
{
    [Fact]
    public void Ctor()
    {
        var manager = new MemoryPlayInfoRepository(TestPlayInfoFactory.Create());

        Assert.Null(manager.Value);
    }

    [Fact]
    public void Ctor_NullValue_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new MemoryPlayInfoRepository(null!));
    }

    [Fact]
    public void LoadAsync()
    {
        var value = TestPlayInfoFactory.Create();
        var manager = new MemoryPlayInfoRepository(value);

        var task = manager.LoadAsync(default);

        Assert.True(task.IsCompletedSuccessfully);
        Assert.Equal(value, manager.Value);
    }
}
