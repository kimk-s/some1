using System;
using Some1.Play.Data;

namespace Some1.Data.InMemory.Internal
{
    internal class UserData
    {
        public string Id { get; set; } = "";
        public string PlayId { get; set; } = "";
        public bool IsPlaying { get; set; }
        public bool Manager { get; set; }
        public int PremiumAccumulatedDays { get; set; }
        public DateTime? PremiumExpirationDate { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? SaveTime { get; set; }
        public PackedData? PackedData { get; set; }
    }
}
