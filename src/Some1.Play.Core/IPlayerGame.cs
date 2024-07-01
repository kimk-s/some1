using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IPlayerGame
    {
        Game? Game { get; }
        float Cycles { get; }
    }
}
