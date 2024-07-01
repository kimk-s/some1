using System;
using System.Threading;
using System.Threading.Tasks;
using Some1.Auth.Front;
using Some1.Play.Client;

namespace Some1.User.Unity
{
    public sealed class PlayClientAuth : IPlayClientAuth
    {
        private readonly IAuthFront _authFront;

        public PlayClientAuth(IAuthFront authFront)
        {
            _authFront = authFront;
        }

        public Task<string> GetIdTokenAsync(bool forceRefresh, CancellationToken cancellationToken)
            => GetAuthUser().GetIdTokenAsync(forceRefresh, cancellationToken);

        public string GetUserId() => GetAuthUser().UserId;

        private IAuthUserFront GetAuthUser() => _authFront.User.CurrentValue ?? throw new InvalidOperationException();
    }
}
