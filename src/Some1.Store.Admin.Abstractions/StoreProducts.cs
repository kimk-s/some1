using System;

namespace Some1.Store.Admin
{
    public static class StoreProducts
    {
        public static int GetPremium(string productId) => productId switch
        {
            "premium30d" => 30,
            "premium90d" => 90,
            "premium180d" => 180,
            _ => throw new ArgumentOutOfRangeException(nameof(productId))
        };
    }
}
