using MagicOnion;
using MagicOnion.Server;

namespace Some1.Wait.Back.MagicServer
{
    [FromTypeFilter(typeof(VerifyIdTokenFilterAttribute))]
    public class WaitBackMagicService : ServiceBase<IWaitBackMagicService>, IWaitBackMagicService
    {
        private readonly WaitBack _back;

        public WaitBackMagicService(WaitBack back)
        {
            _back = back;
        }

        private CancellationToken CancellationToken => Context.CallContext.CancellationToken;

        public async UnaryResult<WaitUserBack> GetUserAsync()
        {
            return await _back.GetUserAsync(GetUserId(), CancellationToken).ConfigureAwait(false);
        }

        public async UnaryResult<WaitWaitBack> GetWaitAsync()
        {
            return await _back.GetWaitAsync(CancellationToken).ConfigureAwait(false);
        }

        public async UnaryResult<WaitPlayBack[]> GetPlaysAsync()
        {
            return await _back.GetPlaysAsync(CancellationToken).ConfigureAwait(false);
        }

        public async UnaryResult<WaitPremiumLogBack[]> GetOrdersAsync()
        {
            return await _back.GetPremiumLogsAsync(GetUserId(), CancellationToken).ConfigureAwait(false);
        }

        public async UnaryResult BuyProductAsync((string productId, string transactionId) x)
        {
            await _back.BuyProductAsync(x.productId, x.transactionId, GetUserId(), CancellationToken).ConfigureAwait(false);
        }

        private string GetUserId() => (string)Context.CallContext.UserState["user-id"];
    }
}
