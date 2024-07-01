using System;

namespace Some1.Data.InMemory.Internal
{
    internal class PlayData
    {
        public string Id { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string City { get; set; } = null!;
        public int Number { get; set; }
        public string Address { get; set; } = null!;
        public bool OpeningSoon { get; set; }
        public bool Maintenance { get; set; }
        public float Busy { get; set; }
        public DateTime SetTime { get; set; }
    }
}
