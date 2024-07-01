using System;
using System.Collections.Generic;
using R3;
using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IObjectTakeTakeStuffGroup
    {
        event EventHandler<Stuff>? Added;

        IReadOnlyList<IObjectTakeStuff> All { get; }
        ReadOnlyReactiveProperty<int> ComboScore { get; }
        float ComboCycles { get; }
    }
}
