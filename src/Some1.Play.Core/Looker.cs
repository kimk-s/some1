using Some1.Play.Info;

namespace Some1.Play.Core
{
    public readonly struct Looker
    {
        public Looker(Area area, byte team)
        {
            Area = area;
            Team = team;
        }

        public Area Area { get; }

        public byte Team { get; }
    }
}
