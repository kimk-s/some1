using System.Numerics;

namespace Some1.Play.Core.Internal
{
    internal interface IInternalObjectRegion : IObjectRegion
    {
        void Set(Vector2 position);
    }

    internal sealed class ObjectRegion : IInternalObjectRegion
    {
        private readonly RegionGroup _regions;
        private Vector2 _position;
        private IRegionSection? _section;

        internal ObjectRegion(RegionGroup regions)
        {
            _regions = regions;
        }

        public IRegionSection? Section => _section;

        public void Set(Vector2 position)
        {
            if (_position == position)
            {
                return;
            }
            _position = position;
            _regions.Refresh(position, ref _section);
        }

        internal void Reset()
        {
            _position = default;
            _section = null;
        }
    }
}
