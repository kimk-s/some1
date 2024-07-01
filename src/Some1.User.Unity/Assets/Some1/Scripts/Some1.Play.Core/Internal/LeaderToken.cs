using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class LeaderToken : ILeaderToken
    {
        public Area Area { get; private set; }

        internal void SetArea(Area area)
        {
            Area = area;
        }

        internal void ResetArea()
        {
            Area = default;
        }
    }
}
