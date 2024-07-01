using System.Diagnostics.CodeAnalysis;

namespace Some1.Play.Core.Paralleling;

public class ParallelCollectionTests
{
    [Fact]
    public void Ctor()
    {
        var parallelCount = 1;

        var collection = new ParallelCollection<int>(new() { Count = parallelCount });
        Assert.Empty(collection);
        Assert.Equal(parallelCount, collection.ParallelCount);
    }

    [Fact]
    public void Ctor_NullArgument_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ParallelCollection<int>(null!));
    }

    [Fact]
    [SuppressMessage("Assertions", "xUnit2013:Do not use equality check to check for collection size.", Justification = "<Pending>")]
    public void Add()
    {
        var collection = new ParallelCollection<int>(new() { Count = 1 });

        collection.Add(1);
        Assert.Equal(1, collection.Count);
        Assert.Equal(new int[] { 1 }, collection);

        collection.Add(2);
        Assert.Equal(2, collection.Count);
        Assert.Equal(new int[] { 1, 2 }, collection);

        collection.Add(3);
        Assert.Equal(3, collection.Count);
        Assert.Equal(new int[] { 1, 2, 3 }, collection);

        collection.Add(4);
        Assert.Equal(4, collection.Count);
        Assert.Equal(new int[] { 1, 2, 3, 4 }, collection);
    }

    [Fact]
    [SuppressMessage("Assertions", "xUnit2013:Do not use equality check to check for collection size.", Justification = "<Pending>")]
    public void Add_ParallelToken()
    {
        var s = Services.Create(2);

        s.Collection.Add(1, s.ParallelTokens.All[0]);
        Assert.Equal(1, s.Collection.Count);
        Assert.Equal(new int[] { 1 }, s.Collection);

        s.Collection.Add(2, s.ParallelTokens.All[1]);
        Assert.Equal(2, s.Collection.Count);
        Assert.Equal(new int[] { 1, 2 }, s.Collection);

        s.Collection.Add(3, s.ParallelTokens.All[0]);
        Assert.Equal(3, s.Collection.Count);
        Assert.Equal(new int[] { 1, 3, 2 }, s.Collection);

        s.Collection.Add(4, s.ParallelTokens.All[1]);
        Assert.Equal(4, s.Collection.Count);
        Assert.Equal(new int[] { 1, 3, 2, 4 }, s.Collection);
    }

    [Fact]
    [SuppressMessage("Assertions", "xUnit2013:Do not use equality check to check for collection size.", Justification = "<Pending>")]
    public void Remove()
    {
        var s = Services.CreateWithData();

        s.Collection.Remove(1);
        Assert.Equal(3, s.Collection.Count);
        Assert.Equal(new int[] { 3, 2, 4 }, s.Collection);

        s.Collection.Remove(2);
        Assert.Equal(2, s.Collection.Count);
        Assert.Equal(new int[] { 3, 4 }, s.Collection);

        s.Collection.Remove(3);
        Assert.Equal(1, s.Collection.Count);
        Assert.Equal(new int[] { 4 }, s.Collection);

        s.Collection.Remove(4);
        Assert.Equal(0, s.Collection.Count);
        Assert.Empty(s.Collection);
    }

    [Fact]
    [SuppressMessage("Assertions", "xUnit2013:Do not use equality check to check for collection size.", Justification = "<Pending>")]
    public void Clear()
    {
        var s = Services.CreateWithData();

        s.Collection.Clear();

        Assert.Equal(0, s.Collection.Count);
        Assert.Empty(s.Collection);
    }

    [Fact]
    public void Contains()
    {
        var s = Services.CreateWithData();

        foreach (var item in s.Collection)
        {
            Assert.Contains(item, s.Collection);
        }
    }

    [Fact]
    public void CopyTo()
    {
        var s = Services.CreateWithData();
        var destination = new int[4];

        s.Collection.CopyTo(destination, 0);

        Assert.Equal(s.Collection, destination);
    }

    private sealed class Services
    {
        private Services(ParallelCollection<int> parallelCollection, ParallelTokenGroup parallelTokens)
        {
            Collection = parallelCollection;
            ParallelTokens = parallelTokens;
        }

        public ParallelCollection<int> Collection { get; }

        public ParallelTokenGroup ParallelTokens { get; }

        public static Services Create(int parallelCount)
        {
            var parallelOptions = new ParallelOptions() { Count = parallelCount };
            var collection = new ParallelCollection<int>(parallelOptions);
            var parallelTokens = new ParallelTokenGroup(parallelOptions);
            return new(collection, parallelTokens);
        }

        public static Services CreateWithData()
        {
            var s = Create(2);
            s.Collection.Add(1, s.ParallelTokens.All[0]);
            s.Collection.Add(2, s.ParallelTokens.All[1]);
            s.Collection.Add(3, s.ParallelTokens.All[0]);
            s.Collection.Add(4, s.ParallelTokens.All[1]);
            return s;
        }
    }
}
