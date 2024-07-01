using Some1.Play.Client;

namespace Some1.User.CLI;

public sealed class GuidPlayClientAuth : IPlayClientAuth
{
    private readonly string _userId = Guid.NewGuid().ToString();

    public string GetUserId()
    {
        return _userId;
    }

    public Task<string> GetIdTokenAsync(bool forceRefresh, CancellationToken cancellationToken)
    {
        return Task.FromResult(GetUserId());
    }
}

