using Some1.Play.Client.Tcp;

namespace Some1.User.CLI;

public sealed class PlayAddress : IPlayAddressGettable
{
    private string? _address;

    public void SetAddress(string address)
    {
        _address = address;
    }

    public string GetAddress()
    {
        return _address ?? throw new InvalidOperationException();
    }
}

