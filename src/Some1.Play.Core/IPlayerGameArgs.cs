using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IPlayerGameArgs
    {
        GameMode Id { get; }
        bool IsSelected { get; }
        int Score { get; }
    }
}
