using System;
using System.Collections.Generic;
using System.Linq;
using Some1.Play.Front;
using Some1.Play.Info;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class PlayRegionViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayRegionViewModel(IRegionFront front)
        {
            Id = front.Id;
            Area = front.Area;
            Sections = front.Sections.Select(x => new PlayRegionSectionViewModel(x)).ToArray();
        }

        public RegionId Id { get; }

        public Area Area { get; }

        public IReadOnlyList<PlayRegionSectionViewModel> Sections { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
