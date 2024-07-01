namespace Some1.Auth.Admin
{
    public interface IAuthAdmin
    {
        Task InitialzieAsync(string credentialPath, CancellationToken cancellationToken);
        Task<VerifyIdTokenResult> VerifyIdTokenAsync(string token, CancellationToken cancellationToken);
    }

    public record VerifyIdTokenResult(VerifyIdTokenErrorCode ErrorCode, AuthToken? Token = null);

    public enum VerifyIdTokenErrorCode
    {
        Success,
        InvalidToken,
        ExpiredToken,
        Error,
    }
}
