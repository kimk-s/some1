using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IObjectTakeStuff
    {
        Stuff? Stuff { get; }
        int Token { get; }
        FloatWave Cycles { get; }
    }
}
