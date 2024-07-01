using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IObjectBuff
    {
        BuffId? Id { get; }
        int RootId { get; }
        SkinId? SkinId { get; }
        int OffenseStat { get; }
        Trait Trait { get; }
        byte Team { get;}
        int Token { get; }
        IObjectCycles Cycles { get; }
    }
}
