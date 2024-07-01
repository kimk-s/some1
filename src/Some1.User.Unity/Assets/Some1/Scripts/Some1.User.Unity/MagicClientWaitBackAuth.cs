using System.Threading;
using System.Threading.Tasks;
using Some1.Auth.Front;
using Some1.Wait.Back.MagicClient;

namespace Some1.User.Unity
{
    public sealed class MagicClientWaitBackAuth : IMagicClientWaitBackAuth
    {
        private readonly IAuthFront _authFront;

        public MagicClientWaitBackAuth(IAuthFront authFront)
        {
            _authFront = authFront;
        }

        public Task<string> GetIdTokenAsync(bool forceRefresh, CancellationToken cancellationToken)
        {
            return _authFront.User.CurrentValue.GetIdTokenAsync(forceRefresh, cancellationToken);
        }
    }
}
