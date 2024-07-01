using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IObjectHit
    {
        HitId? Id { get; }
        int Value { get; }
        int RootId { get; }
        float Angle { get; }
        BirthId? BirthId { get; }
        int Token { get; }
        FloatWave Cycles { get; }
    }
}
