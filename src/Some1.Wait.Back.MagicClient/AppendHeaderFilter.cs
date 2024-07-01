using System;
using System.Threading.Tasks;
using MagicOnion.Client;

namespace Some1.Wait.Back.MagicClient
{
    public class AppendHeaderFilter : IClientFilter
    {
        private readonly IMagicClientWaitBackAuth _auth;

        public AppendHeaderFilter(IMagicClientWaitBackAuth auth)
        {
            _auth = auth;
        }

        public async ValueTask<ResponseContext> SendAsync(RequestContext context, Func<RequestContext, ValueTask<ResponseContext>> next)
        {
            string idToken = await _auth
                .GetIdTokenAsync(false, context.CallOptions.CancellationToken)
                .ConfigureAwait(false);

            const string key = "id-token";
            var header = context.CallOptions.Headers;
            var entry = header.Get(key);

            if (entry is null)
            {
                header.Add(key, idToken);
            }
            else if (entry.Value != idToken)
            {
                header.Remove(entry);
                header.Add(key, idToken);
            }

            return await next(context).ConfigureAwait(false);
        }
    }
}
