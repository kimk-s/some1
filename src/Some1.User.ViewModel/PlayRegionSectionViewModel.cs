using Some1.Play.Front;
using Some1.Play.Info;

namespace Some1.User.ViewModel
{
    public sealed class PlayRegionSectionViewModel
    {
        public PlayRegionSectionViewModel(IRegionSectionFront front)
        {
            Id = front.Id;
            Type = front.Type;
            Area = front.Area;
        }

        public RegionSectionId Id { get; }

        public SectionType Type { get; }

        public Area Area { get; }
    }
}
