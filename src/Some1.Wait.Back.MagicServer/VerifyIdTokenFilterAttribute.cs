using Grpc.Core;
using MagicOnion.Server;
using Some1.Auth.Admin;

namespace Some1.Wait.Back.MagicServer
{
    public class VerifyIdTokenFilterAttribute : MagicOnionFilterAttribute
    {
        private readonly IAuthAdmin _authAdmin;
        private readonly ILogger<VerifyIdTokenFilterAttribute> _logger;

        public VerifyIdTokenFilterAttribute(IAuthAdmin authAdmin, ILogger<VerifyIdTokenFilterAttribute> logger)
        {
            _authAdmin = authAdmin;
            _logger = logger;
        }

        public override async ValueTask Invoke(ServiceContext context, Func<ServiceContext, ValueTask> next)
        {
            const string key = "id-token";
            string idToken = context.CallContext.RequestHeaders.Get(key)?.Value ?? throw new InvalidOperationException();

            var result = await _authAdmin.VerifyIdTokenAsync(idToken, context.CallContext.CancellationToken).ConfigureAwait(false);
            if (result.ErrorCode != VerifyIdTokenErrorCode.Success)
            {
                context.CallContext.Status = new(StatusCode.Unauthenticated, result.ErrorCode.ToString());
                _logger.LogInformation("Failed to verify id token ({IdToken}). VerifyIdTokenErrorCode is {ErrorCode}.", idToken, result.ErrorCode);
                return;
            }

            context.CallContext.UserState.Add("user-id", result.Token!.Uid);

            await next(context).ConfigureAwait(false);
        }
    }
}
