using System.Collections.Generic;
using System.Numerics;
using R3;

namespace Some1.Play.Front
{
    public interface IObjectHitGroupFront
    {
        IReadOnlyList<IObjectHitFront> All { get; }
        ReadOnlyReactiveProperty<Vector2> DamagePush { get; }
        ReadOnlyReactiveProperty<float?> DamageMinimumCycles { get; }
    }
}
