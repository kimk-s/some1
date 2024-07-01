using System;
using Some1.Wait.Data;

namespace Some1.Data.InMemory.Internal
{
    internal class PremiumLogData
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = "";
        public WaitPremiumLogReason Reason { get; set; }
        public DateTime CreatedDate { get; set; }
        public int PremiumChangedDays { get; set; }
        public int PremiumAccumulatedDays { get; set; }
        public DateTime PremiumExpirationDate { get; set; }
        public string? PurchaseOrderId { get; set; }
        public string? PurchaseProductId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? Note { get; set; }
    }
}
