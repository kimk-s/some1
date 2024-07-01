using System.Numerics;
using Some1.Play.Core.Internal;

namespace Some1.Play.Core.TestHelpers;

internal class FakeObjectRegion : IInternalObjectRegion
{
    public IRegionSection? Section { get; }

    public void Set(Vector2 position)
    {
    }
}
