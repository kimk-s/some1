namespace Some1.Data.InMemory
{
    public class InMemoryRepositoryOptions
    {
        public const string InMemoryRepository = "InMemoryRepository";

        public InMemoryRepositoryOptionsUser? User { get; set; }

        public InMemoryRepositoryOptionsWait? Wait { get; set; }

        public InMemoryRepositoryOptionsPlay[] Plays { get; set; } = null!;
    }

    public class InMemoryRepositoryOptionsUser
    {
        public bool IsPlaying { get; set; }
        public string PlayId { get; set; } = "";
        public bool Manager { get; set; }
    }

    public class InMemoryRepositoryOptionsWait
    {
        public bool Maintenance { get; set; }
    }

    public class InMemoryRepositoryOptionsPlay
    {
        public string Id { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string City { get; set; } = null!;
        public int Number { get; set; }
        public string Address { get; set; } = null!;
        public bool OpeningSoon { get; set; }
        public bool Maintenance { get; set; }
        public float Busy { get; set; }
    }
}
