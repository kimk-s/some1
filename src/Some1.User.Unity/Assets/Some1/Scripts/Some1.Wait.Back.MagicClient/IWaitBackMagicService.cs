using MagicOnion;

namespace Some1.Wait.Back
{
    public interface IWaitBackMagicService : IService<IWaitBackMagicService>
    {
        UnaryResult<WaitUserBack> GetUserAsync();
        UnaryResult<WaitWaitBack> GetWaitAsync();
        UnaryResult<WaitPlayBack[]> GetPlaysAsync();
        UnaryResult<WaitPremiumLogBack[]> GetOrdersAsync();
        UnaryResult BuyProductAsync((string productId, string transactionId) x);
    }
}
