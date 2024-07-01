using System;

namespace Some1.Wait.Data
{
    public class WaitPremiumLogData
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = "";
        public WaitPremiumLogReason Reason { get; set; }
        public DateTime CreatedDate { get; set; }
        public int PremiumChangedDays { get; set; }
        public DateTime PremiumExpirationDate { get; set; }
        public string? PurchaseOrderId { get; set; }
        public string? PurchaseProductId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? Note { get; set; }
    }
}
