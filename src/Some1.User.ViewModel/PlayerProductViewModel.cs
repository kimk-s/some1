using System;
using R3;
using Some1.Wait.Front;

namespace Some1.User.ViewModel
{
    public sealed class PlayerProductViewModel : IDisposable
    {
        public PlayerProductViewModel(
            IStoreProduct storeProduct,
            ReactiveCommand<string> buy)
        {
            Id = storeProduct.Id;
            Title = storeProduct.GetLocalizedName();
            Price = storeProduct.LocalizedPriceString;

            Buy = new ReactiveCommand<Unit>();
            Buy.Subscribe(_ => buy.Execute(Id));
        }

        public string Id { get; }

        public string Title { get; }

        public string Price { get; }

        public ReactiveCommand<Unit> Buy { get; }

        public void Dispose()
        {
            Buy.Dispose();
        }
    }
}
