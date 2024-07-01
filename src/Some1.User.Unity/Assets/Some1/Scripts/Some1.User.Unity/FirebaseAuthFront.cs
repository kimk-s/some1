using System;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Auth;
using Google;
using Microsoft.Extensions.Logging;
using Some1.Auth.Front;
using R3;

namespace Some1.User.Unity
{
    public class FirebaseAuthFront : IAuthFront, IDisposable
    {
        private readonly ReactiveProperty<FirebaseAuthUserFront?> _user = new ReactiveProperty<FirebaseAuthUserFront?>();
        private readonly ILogger<FirebaseAuthFront> _logger;
        private FirebaseAuth _auth;

        public FirebaseAuthFront(ILogger<FirebaseAuthFront> logger)
        {
            _auth = FirebaseAuth.DefaultInstance;
            _auth.StateChanged += Auth_StateChanged;
            Auth_StateChanged(this, null);

            User = _user.Select(x => (IAuthUserFront)x).ToReadOnlyReactiveProperty();
            _logger = logger;

            GoogleSignIn.Configuration ??= new GoogleSignInConfiguration
            {
                RequestIdToken = true,
                RequestEmail = true,
                WebClientId = "391152392188-cs16iljcve1h7nhefqpdpf081818rqk2.apps.googleusercontent.com",
            };
        }

        public ReadOnlyReactiveProperty<IAuthUserFront?> User { get; }

        public Task PassAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public async Task SignInWithEmailAndPasswordAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            await _auth.SignInWithEmailAndPasswordAsync(email, password);
        }

        public async Task SignInWithGoogleAsync(CancellationToken cancellationToken = default)
        {
#if !UNITY_EDITOR
            GoogleSignInUser googleSignInUser;
            try
            {
                googleSignInUser = await GoogleSignIn.DefaultInstance.SignIn();
            }
            catch (GoogleSignIn.SignInException ex)
            {
                _logger.LogInformation($"GoogleSignIn.SignInException {ex.Status} {ex.Message}");
                return;
            }

            var credential = GoogleAuthProvider.GetCredential(googleSignInUser.IdToken, null);
            await _auth.SignInWithCredentialAsync(credential);
#else
            throw new NotSupportedException("Not support in unity editor. Please run on your device.");
#endif
        }

        public Task SignOutAsync(CancellationToken cancellationToken = default)
        {
            _auth.SignOut();
#if !UNITY_EDITOR
            GoogleSignIn.DefaultInstance.SignOut();
#endif
            return Task.CompletedTask;
        }

        public Task CreateUserWithEmailAndPasswordAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        private void Auth_StateChanged(object sender, EventArgs e)
        {
            if (_auth.CurrentUser != _user.Value?.User)
            {
                bool signedIn = _auth.CurrentUser != null && _auth.CurrentUser.IsValid();

                if (!signedIn && _user.Value?.User != null)
                {
                    // Sign out {_user.Value}
                }

                _user.Value = _auth.CurrentUser is null ? null : new(_auth.CurrentUser);

                if (signedIn)
                {
                    // Sign in {_user.Value}
                }
            }
        }

        public void Dispose()
        {
            _auth.StateChanged -= Auth_StateChanged;
            _auth = null;

            _user.Value = null;
            ((IDisposable)_user).Dispose();
        }
    }
}
