using System.Linq;
using R3;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerObjectRegionFront : IPlayerObjectRegionFront
    {
        internal PlayerObjectRegionFront(IObjectTransformFront transform, IRegionGroupFront regions)
        {
            Section = transform.Position.Select(x => regions.Get(x)).ToReadOnlyReactiveProperty();
        }

        public ReadOnlyReactiveProperty<IRegionSectionFront?> Section { get; }
    }
}
