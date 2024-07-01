using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IObjectBooster
    {
        BoosterId Id { get; }
        int Number { get; }
        FloatWave Cycles { get; }
    }
}
