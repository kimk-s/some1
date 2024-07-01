using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IPlayerTitle
    {
        Leveling StarGrade { get; }
        PlayerId PlayerId { get; }
        Leveling Like { get; }
        Medal Medal { get; }
        Title Title { get; }
    }
}
