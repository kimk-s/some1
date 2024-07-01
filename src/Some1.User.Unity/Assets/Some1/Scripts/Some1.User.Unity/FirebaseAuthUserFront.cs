using System;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Auth;
using Some1.Auth.Front;

namespace Some1.User.Unity
{
    public class FirebaseAuthUserFront : IAuthUserFront
    {
        private readonly FirebaseUser _user;

        public FirebaseAuthUserFront(FirebaseUser user)
        {
            _user = user;
        }

        public string DisplayName => _user.DisplayName;

        public string Email => _user.Email;

        public Uri PhotoUrl => _user.PhotoUrl;

        public string UserId => _user.UserId;

        internal FirebaseUser User => _user;

        public Task<string> GetIdTokenAsync(bool forceRefresh, CancellationToken cancellationToken)
        {
            return _user.TokenAsync(forceRefresh);
        }
    }
}
