using System.Collections.Generic;
using R3;

namespace Some1.Play.Front
{
    public interface IObjectTakeStuffGroupFront
    {
        IReadOnlyList<IObjectTakeStuffFront> All { get; }
        ReadOnlyReactiveProperty<int> ComboScore { get; }
        ReadOnlyReactiveProperty<float> ComboCycles { get; }
    }
}
