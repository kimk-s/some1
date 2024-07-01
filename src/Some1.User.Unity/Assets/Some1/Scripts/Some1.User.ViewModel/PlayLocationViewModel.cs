using System;
using System.Numerics;
using R3;
using Some1.Play.Front;

namespace Some1.User.ViewModel
{
    public sealed class PlayLocationViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayLocationViewModel(IPlayerObjectFront front)
        {
            RegionSection = front.Region.Section;
            Position = front.Transform.Position;
        }

        public ReadOnlyReactiveProperty<IRegionSectionFront?> RegionSection { get; }

        public ReadOnlyReactiveProperty<Vector2> Position { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
