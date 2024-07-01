using Some1.Wait.Back;
using Some1.Wait.Front;

namespace Some1.User.ViewModel
{
    public sealed class PlayPremiumLogViewModel
    {
        public PlayPremiumLogViewModel(WaitPremiumLogBack model, IStoreProduct? product)
        {
            Value = model;
            Title = product?.GetLocalizedName() ?? model.PurchaseProductId;
        }

        public WaitPremiumLogBack Value { get; }

        public string? Title { get; }
    }
}
