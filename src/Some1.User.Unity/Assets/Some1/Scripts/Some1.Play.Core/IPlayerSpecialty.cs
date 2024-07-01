using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IPlayerSpecialty
    {
        SpecialtyId Id { get; }
        int Number { get; }
        byte Star { get; }
    }
}
