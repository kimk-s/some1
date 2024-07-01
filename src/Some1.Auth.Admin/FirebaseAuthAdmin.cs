using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;

namespace Some1.Auth.Admin
{
    public sealed class FirebaseAuthAdmin : IAuthAdmin
    {
        private readonly ILogger<FirebaseAuthAdmin> _logger;

        public FirebaseAuthAdmin(ILogger<FirebaseAuthAdmin> logger)
        {
            _logger = logger;
        }

        public async Task InitialzieAsync(string credentialPath, CancellationToken cancellationToken)
        {
            var credential = await GoogleCredential.FromFileAsync(credentialPath, cancellationToken);

            FirebaseApp.Create(new AppOptions()
            {
                Credential = credential,
                ProjectId = "some1-476d0"
            });
        }

        public async Task<VerifyIdTokenResult> VerifyIdTokenAsync(string token, CancellationToken cancellationToken)
        {
            try
            {
                var firebaseToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token, cancellationToken);
                return new(VerifyIdTokenErrorCode.Success, new(firebaseToken.Uid));
            }
            catch (ArgumentException)
            {
                return new(VerifyIdTokenErrorCode.InvalidToken);
            }
            catch (FirebaseAuthException ex)
            {
                switch (ex.AuthErrorCode)
                {
                    case AuthErrorCode.ExpiredIdToken:
                        return new(VerifyIdTokenErrorCode.ExpiredToken);
                    case AuthErrorCode.InvalidIdToken:
                        return new(VerifyIdTokenErrorCode.InvalidToken);
                    default:
                        {
                            _logger.LogError($"Unexpected FirebaseAuthException.AuthErrorCode '{ex.AuthErrorCode}'.");
                            return new(VerifyIdTokenErrorCode.Error);
                        }
                }
            }
        }
    }
}
