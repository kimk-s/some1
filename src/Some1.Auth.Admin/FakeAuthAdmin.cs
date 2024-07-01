namespace Some1.Auth.Admin
{
    public sealed class FakeAuthAdmin : IAuthAdmin
    {
        public Task InitialzieAsync(string credentialPath, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task<VerifyIdTokenResult> VerifyIdTokenAsync(string idToken, CancellationToken cancellationToken)
        {
            return Task.FromResult<VerifyIdTokenResult>(idToken is not null
                ? new(VerifyIdTokenErrorCode.Success, new(idToken))
                : new(VerifyIdTokenErrorCode.InvalidToken, null));
        }
    }
}
