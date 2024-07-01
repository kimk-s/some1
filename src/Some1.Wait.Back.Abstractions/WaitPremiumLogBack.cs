using System;
using MemoryPack;

namespace Some1.Wait.Back
{
    [MemoryPackable]
    public sealed partial class WaitPremiumLogBack
    {
        public WaitPremiumLogBack(Guid id, string userId, WaitPremiumLogReason reason, DateTime createdDate, int premiumChangedDays, DateTime premiumExpirationDate, string? purchaseOrderId, string? purchaseProductId, DateTime? purchaseDate, string? note)
        {
            Id = id;
            UserId = userId;
            Reason = reason;
            CreatedDate = createdDate;
            PremiumChangedDays = premiumChangedDays;
            PremiumExpirationDate = premiumExpirationDate;
            PurchaseOrderId = purchaseOrderId;
            PurchaseProductId = purchaseProductId;
            PurchaseDate = purchaseDate;
            Note = note;
        }

        public Guid Id { get; set; }
        public string UserId { get; set; } = "";
        public WaitPremiumLogReason Reason { get; set; }
        public DateTime CreatedDate { get; set; }
        public int PremiumChangedDays { get; set; }
        public DateTime PremiumExpirationDate { get; set; }
        public string? PurchaseOrderId { get; set; }
        public string? PurchaseProductId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? Note { get; }
    }
}
