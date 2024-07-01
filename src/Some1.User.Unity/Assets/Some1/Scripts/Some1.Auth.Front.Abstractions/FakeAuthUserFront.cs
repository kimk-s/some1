using System;
using System.Threading;
using System.Threading.Tasks;

namespace Some1.Auth.Front
{
    internal sealed class FakeAuthUserFront : IAuthUserFront
    {
        public FakeAuthUserFront(string displayName, string email, Uri? photoUrl, string userId)
        {
            DisplayName = displayName;
            Email = email;
            PhotoUrl = photoUrl;
            UserId = userId;
        }

        public string DisplayName { get; }

        public string Email { get; }

        public Uri? PhotoUrl { get; }

        public string UserId { get; }

        public Task<string> GetIdTokenAsync(bool forceRefresh, CancellationToken cancellationToken)
        {
            return Task.FromResult(UserId);
        }
    }
}
