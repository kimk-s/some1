using System;
using System.Threading;
using System.Threading.Tasks;

namespace Some1.Auth.Front
{
    public interface IAuthUserFront
    {
        public string DisplayName { get; }
        public string Email { get; }
        public Uri? PhotoUrl { get; }
        public string UserId { get; }

        Task<string> GetIdTokenAsync(bool forceRefresh, CancellationToken cancellationToken);
    }
}
