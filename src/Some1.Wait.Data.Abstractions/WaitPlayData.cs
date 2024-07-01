namespace Some1.Wait.Data
{
    public class WaitPlayData
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
