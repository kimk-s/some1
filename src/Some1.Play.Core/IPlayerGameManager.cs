using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IPlayerGameManager
    {
        GameManagerStatus? Status { get; }
        float Cycles { get; }
    }
}
