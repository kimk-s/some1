using Some1.Data.InMemory;

public class DataOptions
{
    public const string Data = "Data";

    public bool InMemory { get; set; }

    public InMemoryRepositoryOptions? InMemoryRepositoryOptions { get; set; }
}
